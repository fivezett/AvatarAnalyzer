using System.Collections.Generic;
using UnityEngine;

namespace AvatarAnalyzer.FixFunctions
{
    public class DeleteComponent : IFixFunction
    {
        public bool Draw(ObjectItemMG OIMG, ObjectItem OI, Component CMP, List<Component> TARGET_CMP)
        {
            if (EZ_UIDrawer.BTN(FixUICode.DeleteComponent))
            {
                Fix(OIMG, OI, CMP, TARGET_CMP);
                return true;
            }
            return false;
        }

        public void Fix(ObjectItemMG OIMG, ObjectItem OI, Component CMP, List<Component> TARGET_CMP)
        {
            Object.DestroyImmediate(CMP);
        }
    }
}