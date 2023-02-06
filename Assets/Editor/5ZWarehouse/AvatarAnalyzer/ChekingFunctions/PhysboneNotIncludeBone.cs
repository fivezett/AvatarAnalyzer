using System.Linq;
using UnityEngine;
using VRC.SDK3.Dynamics.PhysBone.Components;

namespace AvatarAnalyzer.CheckingFunctions
{
    public class PhysboneNotIncludeBone : ICheckingFunction
    {
        /// <summary>
        /// FindBoneAndNullMaterialに依存
        /// Physbone配下にボーンがないか、メッシュレンダラーがない場合に警告
        /// </summary>
        public PhysboneNotIncludeBone() { }
        public void check(ObjectItemMG OIMG)
        {
            //Physboneコンポーネントがあたっているオブジェクトかその配下にボーンがない場合に警告
            OIMG.GetHasComponentObjects<VRCPhysBone>()
                .Where(OI => !OI.obj.transform.GetComponentsInChildren<Transform>()
                .Any(t => OIMG.Get(t.gameObject)
                .HasAttribute(InfoType.Normal, ObjectItem.QuickCreateKey(InformationCode.Bone)) || OIMG.Get(t.gameObject).hasComponent<MeshRenderer>() || OIMG.Get(t.gameObject).hasComponent<ParticleSystem>()))
                .ToList().ForEach(OI => OI.AddAttribute(InfoType.Warn, ObjectItem.QuickCreateKey(InformationCode.PhysboneNotReferenced, OI.getComponent<VRCPhysBone>())));
        }
    }
}