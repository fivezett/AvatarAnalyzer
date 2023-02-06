using AvatarAnalyzer;
using System.Collections.Generic;

namespace AvatarAnalyzer.FixFunctions
{
    public class FixFunctionsList
    {
        Dictionary<InformationCode, List<IFixFunction>> FuncList = new Dictionary<InformationCode, List<IFixFunction>>();
        public Dictionary<InformationCode, List<IFixFunction>> Get()
        {
            QuickList(InformationCode.PhysboneHasNullCollder, new PhysboneHasNullCollder());
            QuickList(InformationCode.PhysboneColliderNotReferenced, new DeleteComponent());
            QuickList(InformationCode.PhysboneColliderOutOfRange, new DeletePhysboneColliderOutOfRange());
            QuickList(InformationCode.PhysboneColliderOutOfRange, new TryReplacePhysboneCollider());
            QuickList(InformationCode.ConstraintHasNull, new ConstraintHasNull());
            QuickList(InformationCode.NonMeanConstraint, new DeleteComponent());
            QuickList(InformationCode.ConstraintOutOfRange, new ConstraintDeleteOutOfRange());
            QuickList(InformationCode.HasAnimatorController, new HasAnimatorController());
            QuickList(InformationCode.PhysboneRadiusIsZero, new PhysboneRadiusIsZero());
            QuickList(InformationCode.NullMaterialSkinnedMesh, new NullMaterialSkinnedMesh());
            QuickList(InformationCode.HasntSkinnedMesh, new DeleteComponent());
            QuickList(InformationCode.PhysboneNotReferenced, new DeleteComponent());
            QuickList(InformationCode.SkinnedMeshRendererHasntBone, new DeleteComponent());
            QuickList(InformationCode.HasntSkinnedMesh, new DeleteComponent());
            QuickList(InformationCode.EyeLookLeftOutOfRange, new TryReplaceLeftEyeTransform());
            QuickList(InformationCode.FaceMeshOutOfRange, new TryReplaceFaceMeshTransform());
            QuickList(InformationCode.EyeLookRightOutOfRange, new TryReplaceRightEyeTransform());

            QuickList(InformationCode.HasntVRCAvatarDescriptor, new AddAvatarDescripter());
            QuickList(InformationCode.HasMultipleVRCAvatarDescriptor, new DeleteComponent());
            QuickList(InformationCode.HasMultipleVRCPipelineManager, new DeleteComponent());


            return FuncList;
        }
        public void QuickList(InformationCode Code, IFixFunction IFF)
        {
            if (!FuncList.ContainsKey(Code))
                FuncList.Add(Code, new List<IFixFunction> { IFF });
            else
                FuncList[Code].Add(IFF);
        }
    }
}