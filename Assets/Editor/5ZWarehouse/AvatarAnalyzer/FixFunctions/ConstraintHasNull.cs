using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace AvatarAnalyzer.FixFunctions
{
    //Constraint Null delete
    public class ConstraintHasNull : IFixFunction
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
            IConstraint constraint = CMP as IConstraint;
            List<ConstraintSource> list = new List<ConstraintSource>();
            for (int i = 0; i < constraint.sourceCount; i++)
                if (constraint.GetSource(i).sourceTransform != null)
                    list.Add(constraint.GetSource(i));
            constraint.SetSources(list);
        }
    }
}