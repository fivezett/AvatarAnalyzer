using AvatarAnalyzer;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AvatarAnalyzer.FixFunctions
{
    public class NullMaterialSkinnedMesh : IFixFunction
    {
        public bool Draw(ObjectItemMG OIMG, ObjectItem OI, Component CMP, List<Component> TARGET_CMP)
        {
            if (EZ_UIDrawer.BTN(FixUICode.DeleteNull))
            {
                Fix(OIMG, OI, CMP, TARGET_CMP);
                return true;
            }
            return false;
        }

        public  void Fix(ObjectItemMG AAOIMG, ObjectItem AAOI, Component CMP, List<Component> TARGET_CMP)
        {
            if (AAOI.hasComponent<SkinnedMeshRenderer>())
            {
                SkinnedMeshRenderer SMR = CMP as SkinnedMeshRenderer;
                SMR.sharedMaterials = new List<Material>(SMR.sharedMaterials).Where(M=>M!=null).ToArray();
            }
        }
    }
}
