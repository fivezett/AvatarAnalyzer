using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using AvatarAnalyzer.CheckingFunctions;
using UnityEditor;

namespace AvatarAnalyzer
{
    public class ObjectItemMG
    {
        /// <summary>
        /// ObjectItemリスト(obj,ObjItem)
        /// </summary>
        public Dictionary<GameObject, ObjectItem> ObjectList = new Dictionary<GameObject, ObjectItem>();

        /// <summary>
        /// 検索量をへらすためのO(1)キャッシュ
        /// </summary>
        public Dictionary<System.Type, List<ObjectItem>> ComponentList = new Dictionary<System.Type, List<ObjectItem>>();
        public Animator AvatarAnimator;
        public GameObject RootObject;
        public List<ICheckingFunction> unitCheckFunctionList;
        public IEnumerable<GameObject> Prefabs;
        public ObjectItemMG(Animator avatarAnimator = null)
        {
            this.AvatarAnimator = avatarAnimator;
            unitCheckFunctionList = new List<ICheckingFunction>
            {
            new HasAnimationController(),
            new PipelineMGCheck(),
            new AvatarDiscripterCheck(),
            new FindBoneAnchorAndNullMaterial(),
            new CheckPhysBone(),
            new CheckConstraint(),
            new SearchNotUsePhysboneCollider(),
            new PhysboneNotIncludeBone(),
            new SearchMeshProbeAnchor(),
            new HasntDependFinalize(),
        };
            Refresh();
        }


        public void Refresh()
        {
            if (AvatarAnimator == null) return;

            ObjectList.Clear();
            ComponentList.Clear();

            //全部突っ込む
            var AllObjects = AvatarAnimator.gameObject.GetComponentsInChildren<Transform>(true).Where(x => x != null).ToList();

            //Prefab検出
            Prefabs = AllObjects.Where(TRS => TRS != null).Select(TRS => TRS.gameObject)
                .Where(OBJ => OBJ != null && OBJ.transform != null &&
                PrefabUtility.GetPrefabAssetType(OBJ) != PrefabAssetType.NotAPrefab)
                .Where(OBJ => OBJ.transform.parent == null || PrefabUtility.GetPrefabAssetType(OBJ.transform.parent.gameObject) == PrefabAssetType.NotAPrefab);

            AllObjects.ForEach(child => ObjectList.Add(child.gameObject, new ObjectItem(child.gameObject)));

            //Root属性を付与
            Get(AvatarAnimator).AddAttribute(InfoType.Normal, ObjectItem.QuickCreateKey(InformationCode.Root));

            //Armature(という名前があれば)属性を付与
            //Armature以外の名前の場合は知らん、説明を書いてくれているだけなので
            if (Has(RootObject.transform.Find("Armature")))
                Get(RootObject.transform.Find("Armature")).AddAttribute(InfoType.Normal, ObjectItem.QuickCreateKey(InformationCode.Armature));

            //単体で完結するチェック
            unitCheckFunctionList.ForEach(func => func.check(this));
        }

        public bool Has(GameObject obj)
        {
            if (obj == null) return false;
            return ObjectList.ContainsKey(obj);
        }

        public bool Has(Component cmp)
        {
            if (cmp == null) return false;
            return Has(cmp.gameObject);
        }

        public ObjectItem Get(GameObject obj)
        {
            return ObjectList[obj];
        }
        public ObjectItem Get(Component cmp)
        {
            return Get(cmp.gameObject);
        }

        public int GetWarnAttributeSize()
        {
            int size = 0;
            foreach (var objKV in ObjectList)
                size += objKV.Value.warnAttribute.Count;
            return size;
        }

        public int GetBadAttributeSize()
        {
            int size = 0;
            foreach (var objKV in ObjectList)
                size += objKV.Value.badAttribute.Count;
            return size;
        }

        public int GetDeleteAttributeSize()
        {
            int size = 0;
            foreach (var objKV in ObjectList)
                if (!objKV.Value.hasDepend())
                    size++;
            return size;
        }

        public void Set(Animator avatarAnimator = null)
        {
            if (this.AvatarAnimator != avatarAnimator)
            {
                this.AvatarAnimator = avatarAnimator;
                this.RootObject = avatarAnimator?.gameObject;
            }
            Refresh();
        }

        public List<ObjectItem> GetHasComponentObjects<T>() where T : UnityEngine.Component
        {
            if (ComponentList.ContainsKey(typeof(T)))
                return ComponentList[typeof(T)];
            else
                return ComponentList[typeof(T)] = ObjectList.Values.Where(x => x.hasComponent<T>()).ToList();
        }
        public List<ObjectItem> GetHasComponentObjects<T>(bool active) where T : UnityEngine.Component
        {
            return GetHasComponentObjects<T>().Where(OI => OI.obj.activeInHierarchy == active).ToList();
        }
    }
}