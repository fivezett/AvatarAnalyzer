using AvatarAnalyzer.EditorFixFunctions;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VRC.Dynamics;
using VRC.SDK3.Dynamics.PhysBone.Components;

namespace AvatarAnalyzer.FixFunctions
{
    public class TryReplacePhysboneCollider : IFixFunction
    {
        public bool Draw(ObjectItemMG OIMG, ObjectItem OI, Component CMP, List<Component> TARGET_CMP)
        {
            if (EZ_UIDrawer.BTN(FixUICode.TryReplaceAll))
            {
                List<string> Ans = TARGET_CMP.Select(TCMP =>
                    TCMP.gameObject.name + ":" + (Replace(OIMG, CMP as VRCPhysBone, TCMP as VRCPhysBoneColliderBase) == false ? Language.GetTra(FixUICode.NotFound) : Language.GetTra(FixUICode.Success))).ToList();
                EditorUtility.DisplayDialog(Language.GetTra(FixUICode.Result), string.Join("\n", Ans), "OK");
                return true;

            }

            foreach (var TCMP in TARGET_CMP)
            {
                VRCPhysBoneColliderBase TCMP_ = TCMP as VRCPhysBoneColliderBase;
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.ObjectField(TCMP, typeof(VRCPhysBoneColliderBase), true);
                    if (EZ_UIDrawer.BTN(FixUICode.TryReplace))
                    {
                        var Ans = Replace(OIMG, CMP as VRCPhysBone, TCMP as VRCPhysBoneColliderBase);
                        EditorUtility.DisplayDialog(Language.GetTra(FixUICode.Result), Ans ? Language.GetTra(FixUICode.Success) : Language.GetTra(FixUICode.NotFound), "OK");
                        return true;
                    }
                }
            }
            return false;
        }

        public bool Replace(ObjectItemMG OIMG, VRCPhysBone BasePB, VRCPhysBoneColliderBase TCMP)
        {
            var OBJ = TryReplaceLib.Get(OIMG.RootObject, TCMP.gameObject);
            if (OBJ == null) return false;
            VRCPhysBoneColliderBase PBC = OBJ.GetComponent<VRCPhysBoneColliderBase>();
            if (PBC == null)
                VRCPhysboneColliderCopy.Run(TCMP, PBC = OBJ.gameObject.AddComponent<VRCPhysBoneCollider>());
            //PBC参照を切り替え
            BasePB.colliders = BasePB.colliders.ToList().Select(C => C == TCMP ? PBC : C).ToList();
            return true;
        }

        public void Fix(ObjectItemMG OIMG, ObjectItem OI, Component CMP, List<Component> TARGET_CMP)
        {

        }
    }
}
