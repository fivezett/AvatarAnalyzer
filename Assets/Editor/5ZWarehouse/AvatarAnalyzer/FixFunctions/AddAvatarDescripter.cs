using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace AvatarAnalyzer.FixFunctions
{
    public class AddAvatarDescripter : IFixFunction
    {
        public bool Draw(ObjectItemMG OIMG, ObjectItem OI, Component CMP, List<Component> TARGET_CMP)
        {
            if (EZ_UIDrawer.BTN(FixUICode.AddAvatarDescripter))
            {
                OIMG.RootObject.AddComponent<VRCAvatarDescriptor>();
                return true;
            }
            return false;
        }

        public void Fix(ObjectItemMG OIMG, ObjectItem OI, Component CMP, List<Component> TARGET_CMP)
        {
        }
    }
}
