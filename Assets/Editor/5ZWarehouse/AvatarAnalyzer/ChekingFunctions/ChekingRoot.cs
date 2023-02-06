using System.Collections.Generic;
using System.Linq;
using VRC.Core;
using VRC.SDK3.Avatars.Components;

namespace AvatarAnalyzer.CheckingFunctions
{

    public class HasAnimationController : ICheckingFunction
    {
        /// <summary>
        /// 依存なし
        /// アバターのアニメーターにコントローラーがあるか見ます
        /// ある場合は、しゃがんだりするので警告します。
        /// </summary>
        public HasAnimationController() { }
        public void check(ObjectItemMG OIMG)
        {
            if (OIMG.AvatarAnimator.runtimeAnimatorController != null)
                OIMG.Get(OIMG.AvatarAnimator)
                    .AddAttribute(InfoType.Warn, ObjectItem.QuickCreateKey(InformationCode.HasAnimatorController, OIMG.AvatarAnimator));
        }
    }

    public class PipelineMGCheck : ICheckingFunction
    {
        /// <summary>
        /// 依存なし
        /// パイプラインが1個以外なら警告します。
        /// </summary>
        public PipelineMGCheck() { }
        public void check(ObjectItemMG OIMG)
        {
            if (OIMG.AvatarAnimator.GetComponents<PipelineManager>().Length == 0)
                OIMG.Get(OIMG.AvatarAnimator).AddAttribute(InfoType.Warn, ObjectItem.QuickCreateKey(InformationCode.HasntVRCPipelineManager));
            else if (OIMG.AvatarAnimator.GetComponents<PipelineManager>().Length != 1)
            {
                OIMG.AvatarAnimator.GetComponents<PipelineManager>().ToList().ForEach(PM =>
                OIMG.Get(OIMG.AvatarAnimator)
                    .AddAttribute(InfoType.Bad, ObjectItem.QuickCreateKey(InformationCode.HasMultipleVRCPipelineManager, PM)));
            }
        }
    }


    public class AvatarDiscripterCheck : ICheckingFunction
    {
        /// <summary>
        /// 依存なし
        /// アバターディスクリプターのメッシュエラー、目のエラー
        /// コンポーネント数のチェックをします。
        /// </summary>
        public AvatarDiscripterCheck() { }

        public void check(ObjectItemMG OIMG)
        {
            //AvatarDescripterのMeshが領域外参照だとアバター切り替え時に非表示になるので警告

            if (OIMG.RootObject.GetComponent<VRCAvatarDescriptor>() != null)
            {
                if (OIMG.RootObject.GetComponents<VRCAvatarDescriptor>().Length != 1)
                {
                    OIMG.RootObject.GetComponents<VRCAvatarDescriptor>().ToList().ForEach(VAD =>
                        OIMG.Get(OIMG.RootObject).AddAttribute(InfoType.Bad, ObjectItem.QuickCreateKey(InformationCode.HasMultipleVRCAvatarDescriptor, VAD)));
                }
                else
                {
                    VRCAvatarDescriptor cmp = OIMG.AvatarAnimator.gameObject.GetComponent<VRCAvatarDescriptor>();
                    //顔メッシュ無い
                    if (cmp.VisemeSkinnedMesh == null)
                        OIMG.Get(OIMG.AvatarAnimator).AddAttribute(InfoType.Warn, ObjectItem.QuickCreateKey(InformationCode.NoFaceMesh));
                    else if (!OIMG.Has(cmp.VisemeSkinnedMesh))
                        OIMG.Get(OIMG.AvatarAnimator).AddAttribute(InfoType.Bad, ObjectItem.QuickCreateKey(InformationCode.FaceMeshOutOfRange, cmp.VisemeSkinnedMesh));

                    //目がない
                    if (cmp.customEyeLookSettings.rightEye == null)
                        OIMG.Get(OIMG.AvatarAnimator).AddAttribute(InfoType.Warn, ObjectItem.QuickCreateKey(InformationCode.NoEyeLookRight));
                    else if (!OIMG.Has(cmp.customEyeLookSettings.rightEye))
                        OIMG.Get(OIMG.AvatarAnimator).AddAttribute(InfoType.Bad, ObjectItem.QuickCreateKey(InformationCode.EyeLookRightOutOfRange, cmp.customEyeLookSettings.rightEye));
                    else
                        OIMG.Get(cmp.customEyeLookSettings.rightEye).AddAttribute(InfoType.Normal, ObjectItem.QuickCreateKey(InformationCode.EyeLookRight));

                    if (cmp.customEyeLookSettings.leftEye == null)
                        OIMG.Get(OIMG.AvatarAnimator).AddAttribute(InfoType.Warn, ObjectItem.QuickCreateKey(InformationCode.NoEyeLookLeft));
                    else if (!OIMG.Has(cmp.customEyeLookSettings.leftEye))
                        OIMG.Get(OIMG.AvatarAnimator).AddAttribute(InfoType.Bad, ObjectItem.QuickCreateKey(InformationCode.EyeLookLeftOutOfRange, cmp.customEyeLookSettings.leftEye));
                    else
                        OIMG.Get(cmp.customEyeLookSettings.leftEye).AddAttribute(InfoType.Normal, ObjectItem.QuickCreateKey(InformationCode.EyeLookLeft));
                }
            }
            else
            {
                //AvatarDescripterがない
                OIMG.Get(OIMG.AvatarAnimator).AddAttribute(InfoType.Bad, ObjectItem.QuickCreateKey(InformationCode.HasntVRCAvatarDescriptor));
            }
        }
    }
}