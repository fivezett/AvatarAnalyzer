using System.Linq;
using UnityEngine;
using VRC.SDK3.Dynamics.PhysBone.Components;

namespace AvatarAnalyzer.CheckingFunctions
{
    public class CheckPhysBone : ICheckingFunction
    {
        /// <summary>
        /// 依存なし
        /// Physboneの子をPhysboneChildとし
        /// Physboneの設定のチェック
        /// Colliderの確認及び参照属性の付与をします
        /// </summary>
        public CheckPhysBone() { }
        public void check(ObjectItemMG OIMG)
        {
            OIMG.GetHasComponentObjects<VRCPhysBone>().ForEach(OI =>
            {
                VRCPhysBone physbone = OI.getComponent<VRCPhysBone>();

                //Physboneの子はPhysboneになる
                OI.obj.GetComponentsInChildren<Transform>(true).ToList()
                .ForEach(Trans => OIMG.Get(Trans).AddAttribute(InfoType.Normal, ObjectItem.QuickCreateKey(InformationCode.PhysboneChild, Trans), physbone));

                if (physbone.colliders.Count != 0)
                {
                    //当たり判定がない
                    if (physbone.radius == 0)
                        OI.AddAttribute(InfoType.Warn, ObjectItem.QuickCreateKey(InformationCode.PhysboneRadiusIsZero, physbone));

                    //当たり判定が無効
                    if (!physbone.allowCollision)
                        OI.AddAttribute(InfoType.Warn, ObjectItem.QuickCreateKey(InformationCode.PhysboneCollisionDisabled, physbone));

                }
                physbone.colliders.ForEach(C =>
                {
                    //PhysboneColliderに参照情報を付与
                    if (OIMG.Has(C))
                        OIMG.Get(C)
                        .AddAttribute(InfoType.Normal, ObjectItem.QuickCreateKey(InformationCode.ReferencedPhysbone, C), physbone);
                    else
                    {
                        //PhysboneColliderがNull,アバター外
                        if (C == null)
                            OI.AddAttribute(InfoType.Warn, ObjectItem.QuickCreateKey(InformationCode.PhysboneHasNullCollder, physbone));
                        else
                            OI.AddAttribute(InfoType.Warn, ObjectItem.QuickCreateKey(InformationCode.PhysboneColliderOutOfRange, physbone), C);
                    }
                });
            });
        }
    }
}