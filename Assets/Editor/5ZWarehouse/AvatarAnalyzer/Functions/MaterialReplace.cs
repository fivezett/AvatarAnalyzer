using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AvatarAnalyzer
{
    public class MaterialReplace
    {
        public static void Run(ObjectItemMG OIMG, Material FromMAT, Material ToMAT)
        {
            OIMG.GetHasComponentObjects<MeshRenderer>().Select(OI => OI.getComponent<MeshRenderer>())
                .ToList().ForEach(MR =>
                {
                    MR.sharedMaterials = MR.sharedMaterials.Select(M => M == FromMAT ? ToMAT : M).ToArray();
                });
            OIMG.GetHasComponentObjects<SkinnedMeshRenderer>().Select(OI => OI.getComponent<SkinnedMeshRenderer>())
                .ToList().ForEach(SMR =>
                {
                    SMR.sharedMaterials = SMR.sharedMaterials.Select(M => M == FromMAT ? ToMAT : M).ToArray();
                });
        }
    }
}
