using System.Linq;
using UnityEngine;

namespace AvatarAnalyzer.CheckingFunctions
{
    public class HasntDependFinalize : ICheckingFunction
    {
        /// <summary>
        /// 最後に実行
        /// 子に有用なオブジェクトがあるか、~~_endはモデリングソフトの仕様なので、有効扱い
        /// </summary>
        public HasntDependFinalize() { }
        public void check(ObjectItemMG OIMG)
        {
            //重要でないと思われるオブジェクトのファイナライズ
            OIMG.ObjectList.Values.ToList().Where(x => !x.hasDepend()).ToList().ForEach(OI =>
            {
                //子持ちは除外
                if (OI.obj.GetComponentsInChildren<Transform>(true).ToList().Any(x => OIMG.Get(x).hasDepend()))
                    OI.AddAttribute(InfoType.Normal, ObjectItem.QuickCreateKey(InformationCode.HasChild));

                //ボーンの最後はモデリングソフトの仕様でついてくることがあるので自由として警告
                Transform parent = OI.obj.transform.parent;
                if (parent != null && OIMG.ObjectList.ContainsKey(parent.gameObject))
                    if (OIMG.Get(parent.gameObject).HasAttribute(InfoType.Normal, ObjectItem.QuickCreateKey(InformationCode.Bone)))
                        if (OI.obj.name.EndsWith("end"))
                            OI.AddAttribute(InfoType.Normal, ObjectItem.QuickCreateKey(InformationCode.BoneEnd));
            });
        }

    }
}