using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AvatarAnalyzer
{
    public class ProbeAnchorOverider
    {
        public GameObject Anchor;
        public void Run(ObjectItemMG OIMG, bool MessageON)
        {
            if (Anchor != null && !OIMG.Has(Anchor))
            {
                EditorUtility.DisplayDialog("AvatarAnalyzer", Language.GetTra(UICode.AnchorTransformNotFoundInSelected), "OK");
                return;
            }
            OIMG.GetHasComponentObjects<SkinnedMeshRenderer>()
                .Select(OI => OI.getComponent<SkinnedMeshRenderer>()).ToList().ForEach(SMR => SMR.probeAnchor = Anchor?.transform);

            OIMG.GetHasComponentObjects<MeshRenderer>()
                .Select(OI => OI.getComponent<MeshRenderer>()).ToList().ForEach(MR => MR.probeAnchor = Anchor?.transform);

            EditorUtility.DisplayDialog("AvatarAnalyzer", "OK", "OK");
        }
    }
}