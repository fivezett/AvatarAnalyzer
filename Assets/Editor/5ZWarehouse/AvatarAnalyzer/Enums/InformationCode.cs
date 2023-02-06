using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarAnalyzer
{
    public enum InformationCode
    {
        Root,
        Armature,
        IsEditorOnly,
        HasChild,
        DependentSkinnedMeshRenderer,
        Bone,
        SkinnedMeshRendererHasntBone,
        ReferencedPhysbone,
        PhysboneChild,
        ReferencedBone,
        ConstraintChild,
        NonMeanConstraint,
        ConstraintHasNull,
        ConstraintOutOfRange,
        HasntDepend,
        NullMaterialSkinnedMesh,
        PhysboneColliderNotReferenced,
        PhysboneNotReferenced,
        PhysboneColliderOutOfRange,
        NoFaceMesh,
        FaceMeshOutOfRange,
        EyeLookRight,
        EyeLookLeft,
        NoEyeLookRight,
        NoEyeLookLeft,
        EyeLookOutOfRange,
        EyeLookRightOutOfRange,
        EyeLookLeftOutOfRange,
        IsProbeAnchor,
        ProbeAnchorOutOfRange,
        HasntVRCAvatarDescriptor,
        HasMultipleVRCAvatarDescriptor,
        HasAnimatorController,
        HasntVRCPipelineManager,
        HasMultipleVRCPipelineManager,
        PhysboneRadiusIsZero,
        PhysboneCollisionDisabled,
        PhysboneHasNullCollder,
        BoneEnd,
        HasntSkinnedMesh,
    }
}
