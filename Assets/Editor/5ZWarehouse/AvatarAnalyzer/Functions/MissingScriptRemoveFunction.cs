using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AvatarAnalyzer
{
    public class MissingScriptRemoveFunction : MonoBehaviour
    {
        public static void Run(ObjectItemMG OIMG, bool MessageON)
        {
            IEnumerable<GameObject> AllObjects = OIMG.ObjectList.Values.Select(OI => OI.obj);
            int removeCount = AllObjects.Sum(OBJ => GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(OBJ));
            AllObjects.ToList().ForEach(OBJ => GameObjectUtility.RemoveMonoBehavioursWithMissingScript(OBJ));
            EditorUtility.DisplayDialog("AvatarAnalyzer", "Delete " + removeCount.ToString() + "Missing Script.", "OK");
        }
    }
}