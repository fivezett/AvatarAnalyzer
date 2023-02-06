using AvatarAnalyzer.EditorFixFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace AvatarAnalyzer.FixFunctions
{
    public class TryReplaceLeftEyeTransform : IFixFunction
    {
        public bool Draw(ObjectItemMG OIMG, ObjectItem OI, Component CMP, List<Component> TARGET_CMP)
        {
            if (EZ_UIDrawer.BTN(FixUICode.TryReplace))
            {
                var obj = TryReplaceLib.Get(OIMG.RootObject, CMP.gameObject);
                if (obj == null)
                {
                    EditorUtility.DisplayDialog(Language.GetTra(FixUICode.Result), Language.GetTra(FixUICode.NotFound), "OK");
                    return false;
                }
                OI.obj.GetComponent<VRCAvatarDescriptor>().customEyeLookSettings.leftEye = obj.transform;
                EditorUtility.DisplayDialog(Language.GetTra(FixUICode.Result), Language.GetTra(FixUICode.Success), "OK");
                return true;
            }
            return false;
        }

        public void Fix(ObjectItemMG OIMG, ObjectItem OI, Component CMP, List<Component> TARGET_CMP)
        {
        }
    }
}
