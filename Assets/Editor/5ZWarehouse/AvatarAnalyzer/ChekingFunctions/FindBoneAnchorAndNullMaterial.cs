using AvatarAnalyzer;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AvatarAnalyzer.CheckingFunctions
{
    public class FindBoneAnchorAndNullMaterial : ICheckingFunction
    {
        /// <summary>
        /// 依存なし
        /// SMRが依存するボーンに属性付与
        /// SMRがMeshを持たない場合に警告
        /// Nullマテリアルを警告
        /// Meshがボーンを持たない場合にも警告
        /// </summary>
        public FindBoneAnchorAndNullMaterial() { }
        /// <summary>
        /// SkinnedMeshRenderer対Mesh Meshが変わったかどうかの判定用
        /// </summary>
        public Dictionary<SkinnedMeshRenderer, Mesh> RendMesh = new Dictionary<SkinnedMeshRenderer, Mesh>();
        /// <summary>
        /// ボーン数カウント用
        /// </summary>
        public Dictionary<Mesh, HashSet<int>> BoneIndexCount = new Dictionary<Mesh, HashSet<int>>();
        public void check(ObjectItemMG OIMG)
        {
            OIMG.GetHasComponentObjects<SkinnedMeshRenderer>().ForEach(OI =>
            {
                SkinnedMeshRenderer SMR = OI.getComponent<SkinnedMeshRenderer>();
                if (SMR.probeAnchor != null)
                {
                    if (!OIMG.Has(SMR.probeAnchor))
                        OI.AddAttribute(InfoType.Warn, ObjectItem.QuickCreateKey(InformationCode.ProbeAnchorOutOfRange,SMR));
                    else
                        OIMG.Get(SMR.probeAnchor).AddAttribute(InfoType.Normal, ObjectItem.QuickCreateKey(InformationCode.IsProbeAnchor),SMR);
                }
                if (SMR.sharedMesh == null)
                    OI.HasAttribute(InfoType.Warn, ObjectItem.QuickCreateKey(InformationCode.HasntSkinnedMesh));
                else
                {
                    //メッシュのキャッシュ
                    if (!RendMesh.ContainsKey(SMR))
                        RendMesh.Add(SMR, SMR.sharedMesh);

                    //依存ボーン番号の算出
                    //重いのでMeshが変更されたときのみ
                    if (!BoneIndexCount.ContainsKey(SMR.sharedMesh) || SMR.sharedMesh != RendMesh[SMR])
                    {
                        RendMesh[SMR] = SMR.sharedMesh;
                        HashSet<int> boneIndexs = new HashSet<int>(SMR.sharedMesh.boneWeights
                            .ToList().SelectMany(b => new int[] { b.boneIndex0, b.boneIndex1, b.boneIndex2, b.boneIndex3 }));
                        BoneIndexCount[SMR.sharedMesh] = boneIndexs;
                    }

                    //依存ボーン属性付け
                    var boneTransList = BoneIndexCount[SMR.sharedMesh].Where(x => OIMG.Has(SMR.bones[x])).ToList()
                    .Select(boneIndex => SMR.bones[boneIndex]).ToList();
                    boneTransList.ForEach(boneTrans =>
                    {
                        OIMG.Get(boneTrans).AddAttribute(InfoType.Normal, ObjectItem.QuickCreateKey(InformationCode.DependentSkinnedMeshRenderer), SMR);
                        OIMG.Get(boneTrans).AddAttribute(InfoType.Normal, ObjectItem.QuickCreateKey(InformationCode.Bone));
                        OI.AddAttribute(InfoType.Normal, ObjectItem.QuickCreateKey(InformationCode.ReferencedBone, SMR), boneTrans);
                    });

                    //ボーンが存在しない
                    if (boneTransList.Count == 0)
                        OI.AddAttribute(InfoType.Warn, ObjectItem.QuickCreateKey(InformationCode.SkinnedMeshRendererHasntBone));
                }

                //Null Material
                if (SMR.sharedMaterials.ToList().Contains(null))
                    OI.AddAttribute(InfoType.Warn, ObjectItem.QuickCreateKey(InformationCode.NullMaterialSkinnedMesh));
            });
        }
    }
}