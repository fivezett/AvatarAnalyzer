using AvatarAnalyzer;
using System.Collections.Generic;
using UnityEngine;
using VRC.SDK3.Dynamics.PhysBone.Components;

namespace AvatarAnalyzer.FixFunctions
{
    public class PhysboneHasNullCollder : IFixFunction
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

        public void Fix(ObjectItemMG AAOIMG, ObjectItem AAOI, Component CMP, List<Component> TARGET_CMP)
        {
            VRCPhysBone PB = CMP as VRCPhysBone;
            PB.colliders.RemoveAll(item => item == null);
        }
    }
}
