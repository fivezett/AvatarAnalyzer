using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AvatarAnalyzer.EditorFixFunctions
{
    public class TryReplaceLib
    {
        public static GameObject Get(GameObject AvatarRoot, GameObject TargetObject)
        {
            Transform CurrentObject = TargetObject.transform;
            Animator TargetAnimator = null;
            List<string> History = new List<string>();
            while (CurrentObject.parent != null && TargetAnimator == null)
            {
                History.Add(CurrentObject.name);
                CurrentObject = CurrentObject.parent;
                TargetAnimator = CurrentObject.GetComponent<Animator>();
            }
            if (TargetAnimator == null) return null;
            GameObject Pointer = AvatarRoot;
            History.AsEnumerable().Reverse().ToList().ForEach(name =>
            {
                //計算量そんな変わらんので、ForEachでヨシ
                if (Pointer == null) return;
                Pointer = Pointer.transform.Find(name).gameObject;
            });
            return Pointer;
        }
    }
}
