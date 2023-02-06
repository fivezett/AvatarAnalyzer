using System.Linq;
using VRC.Dynamics;

namespace AvatarAnalyzer.CheckingFunctions
{
    public class SearchNotUsePhysboneCollider : ICheckingFunction
    {
        /// <summary>
        /// CheckPhysBoneが先に必要
        /// 参照されていないPhysboneColliderを検索します
        /// </summary>
        public SearchNotUsePhysboneCollider() { }
        public void check(ObjectItemMG OIMG)
        {
            //PhysboneColliderがPhysboneにより参照されているフラグがない場合に警告
            OIMG.GetHasComponentObjects<VRCPhysBoneColliderBase>()
                .Where(OI => !OI.HasAttribute(InfoType.Normal, ObjectItem.QuickCreateKey(InformationCode.ReferencedPhysbone,OI.getComponent<VRCPhysBoneColliderBase>())))
                .ToList().ForEach(OI => OI.AddAttribute(InfoType.Warn, ObjectItem.QuickCreateKey(InformationCode.PhysboneColliderNotReferenced, OI.getComponent<VRCPhysBoneColliderBase>())));
        }
    }
}