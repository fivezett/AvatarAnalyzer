using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace AvatarAnalyzer.CheckingFunctions
{
    public class CheckConstraint : ICheckingFunction
    {
        /// <summary>
        /// 依存なし
        /// Constraint6コンポーネントのNullCheck,範囲外チェック,参照属性の付与をします
        /// </summary>
        public CheckConstraint() { }
        public void check(ObjectItemMG OIMG)
        {
            CommonCheck<ParentConstraint>(OIMG);
            CommonCheck<PositionConstraint>(OIMG);
            CommonCheck<RotationConstraint>(OIMG);
            CommonCheck<ScaleConstraint>(OIMG);
            CommonCheck<AimConstraint>(OIMG);
            CommonCheck<LookAtConstraint>(OIMG);
        }

        private void CommonCheck<T>(ObjectItemMG OIMG) where T : Component, IConstraint
        {
            OIMG.GetHasComponentObjects<T>().ForEach(OI =>
            {
                T constraint = (OI.getComponent<T>());
                if (constraint.sourceCount == 0)
                    OI.AddAttribute(InfoType.Warn, ObjectItem.QuickCreateKey(InformationCode.NonMeanConstraint, constraint));
                else
                {
                    List<ConstraintSource> CS = new List<ConstraintSource>();
                    constraint.GetSources(CS);
                    CS.ForEach(source =>
                    {
                        if (OIMG.Has(source.sourceTransform))
                            OIMG.Get(source.sourceTransform.gameObject).AddAttribute
                            (InfoType.Normal, ObjectItem.QuickCreateKey(InformationCode.ConstraintChild, OI.obj.transform));
                        else
                        {
                            if (source.sourceTransform == null)
                                OI.AddAttribute(InfoType.Warn, ObjectItem.QuickCreateKey(InformationCode.ConstraintHasNull, constraint));
                            else
                                OI.AddAttribute(InfoType.Warn, ObjectItem.QuickCreateKey(InformationCode.ConstraintOutOfRange, constraint), source.sourceTransform);
                        }
                    });
                }
            });

        }
    }
}