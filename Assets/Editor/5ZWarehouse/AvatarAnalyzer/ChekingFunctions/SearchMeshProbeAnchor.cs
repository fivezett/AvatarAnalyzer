using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AvatarAnalyzer.CheckingFunctions
{
    public class SearchMeshProbeAnchor : ICheckingFunction
    {
        public void check(ObjectItemMG OIMG)
        {
            OIMG.GetHasComponentObjects<MeshRenderer>().ToList().ForEach(OI =>
            {
                var MR = OI.getComponent<MeshRenderer>();
                if (MR.probeAnchor != null)
                {
                    if (!OIMG.Has(MR.probeAnchor))
                        OI.AddAttribute(InfoType.Warn, ObjectItem.QuickCreateKey(InformationCode.ProbeAnchorOutOfRange, MR));
                    else
                        OIMG.Get(MR.probeAnchor).AddAttribute(InfoType.Normal, ObjectItem.QuickCreateKey(InformationCode.IsProbeAnchor), MR);
                }
            });
        }
    }
}
