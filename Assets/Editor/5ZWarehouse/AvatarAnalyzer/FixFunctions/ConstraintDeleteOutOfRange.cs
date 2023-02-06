using AvatarAnalyzer;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace AvatarAnalyzer.FixFunctions
{
    //Constraint Out of Range
    public class ConstraintDeleteOutOfRange : IFixFunction
    {
        public bool Draw(ObjectItemMG OIMG, ObjectItem OI, Component CMP, List<Component> TARGET_CMP)
        {
            if (EZ_UIDrawer.BTN(FixUICode.DeleteOutOfRange))
            {
                Fix(OIMG, OI, CMP, TARGET_CMP);
                return true;
            }
            return false;
        }

        public void Fix(ObjectItemMG AAOIMG, ObjectItem AAOI, Component CMP, List<Component> TARGET_CMP)
        {
            var constraint = CMP as IConstraint;
            List<ConstraintSource> list = new List<ConstraintSource>();
            for (int i = 0; i < constraint.sourceCount; i++)
                if (AAOIMG.Has(constraint.GetSource(i).sourceTransform))
                    list.Add(constraint.GetSource(i));
            constraint.SetSources(list);
        }
    }
}