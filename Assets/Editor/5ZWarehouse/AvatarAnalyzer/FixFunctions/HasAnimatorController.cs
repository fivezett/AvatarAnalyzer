using AvatarAnalyzer;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarAnalyzer.FixFunctions
{
    public class HasAnimatorController : IFixFunction
    {
        public bool Draw(ObjectItemMG OIMG, ObjectItem OI, Component CMP, List<Component> TARGET_CMP)
        {
            if (EZ_UIDrawer.BTN(FixUICode.DeleteController))
            {
                Fix(OIMG, OI, CMP, TARGET_CMP);
                return true;
            }
            return false;
        }

        public void Fix(ObjectItemMG AAOIMG, ObjectItem AAOI, Component CMP, List<Component> TARGET_CMP)
        {
            var Anim = CMP as Animator;
            Anim.runtimeAnimatorController = null;
        }
    }
}
