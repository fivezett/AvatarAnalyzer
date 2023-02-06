using System.Reflection;
using UnityEngine;
using VRC.Dynamics;
using VRC.SDK3.Dynamics.PhysBone.Components;

namespace AvatarAnalyzer
{
    public class VRCPhysboneColliderCopy
    {
        public static void Run(VRCPhysBoneColliderBase From, VRCPhysBoneColliderBase To)
        {
            To.rootTransform = From.rootTransform;
            To.shapeType = From.shapeType;
            To.radius = From.radius;
            To.height = From.height;
            To.position = new Vector3(From.position.x, From.position.y, From.position.z);
            To.rotation = new Quaternion(From.rotation.x, From.rotation.y, From.rotation.z, From.rotation.w);
            To.insideBounds = From.insideBounds;
            To.bonesAsSpheres = From.bonesAsSpheres;
        }
    }
}
