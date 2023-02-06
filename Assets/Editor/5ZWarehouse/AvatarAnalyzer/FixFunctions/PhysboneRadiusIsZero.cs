using AvatarAnalyzer;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Dynamics.PhysBone.Components;
using YamlDotNet.Core.Tokens;

namespace AvatarAnalyzer.FixFunctions
{
    public class PhysboneRadiusIsZero : IFixFunction
    {
        private float Value = 0.01f;

        public bool Draw(ObjectItemMG OIMG, ObjectItem OI, Component CMP, List<Component> TARGET_CMP)
        {
            Value = EditorGUILayout.Slider(Value, 0.01f, 0.1f);
            if (EZ_UIDrawer.BTN(FixUICode.SetRadius + Value.ToString()))
            {
                Fix(OIMG, OI, CMP, TARGET_CMP);
                return true;
            }
            return false;
        }

        public void Fix(ObjectItemMG AAOIMG, ObjectItem AAOI, Component CMP, List<Component> TARGET_CMP)
        {
            VRCPhysBone PB = CMP as VRCPhysBone;
            PB.radius = Value;
        }
    }
}
