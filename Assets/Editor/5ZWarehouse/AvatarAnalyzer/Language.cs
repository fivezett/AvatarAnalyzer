using System.Collections.Generic;

namespace AvatarAnalyzer
{
    public class Language
    {
        public static Dictionary<InformationCode, string> InformationCodeTranslate;
        public static Dictionary<UICode, string> UITranslate;
        public static Dictionary<FixUICode, string> FUITranslate;


        static Language()
        {
            InformationCodeTranslate = new Dictionary<InformationCode, string>
            {
                { InformationCode.Root, "アバターの親オブジェクトです。削除してはいけません。" },
                { InformationCode.Armature, "骨格を形成するためのオブジェクトです。削除してはいけません。" },
                { InformationCode.HasChild, "このオブジェクトは影響を与える可能性のある子オブジェクトを持っています。" },
                { InformationCode.DependentSkinnedMeshRenderer, "以下のSkinnedMeshRendererがこのボーンを参照しています。" },
                { InformationCode.Bone, "このオブジェクトはボーンです。" },
                { InformationCode.ReferencedPhysbone, "以下のPhysboneに参照されています。" },
                { InformationCode.PhysboneChild, "Physboneの子オブジェクトです。下記オブジェクトが親です。" },
                { InformationCode.ReferencedBone, "このメッシュレンダラーは以下のボーンオブジェクトを使用しています。" },
                { InformationCode.ConstraintChild, "Constraintで以下のオブジェクトが追従しています。" },
                { InformationCode.NonMeanConstraint, "Constraintがにソースがありません。" },
                { InformationCode.ConstraintHasNull, "Constraintに空のソースが含まれています。" },
                { InformationCode.ConstraintOutOfRange, "Constraintにアバターの範囲外のソースが含まれています。" },
                { InformationCode.HasntDepend, "このオブジェクトは必要とされていない可能性が高いです。" },
                { InformationCode.NullMaterialSkinnedMesh, "マテリアルが設定されていない項目があります。" },
                { InformationCode.PhysboneColliderNotReferenced, "このコライダーはPhysboneに参照されていません。" },
                { InformationCode.PhysboneNotReferenced, "このPhysboneは参照も子ボーンもありません" },
                { InformationCode.PhysboneColliderOutOfRange, "アバターの範囲外にあるコライダーがあります。" },
                { InformationCode.NoFaceMesh, "顔メッシュがありません。" },
                { InformationCode.FaceMeshOutOfRange, "顔メッシュがアバターの範囲外にあります。" },
                { InformationCode.EyeLookRight, "右目です" },
                { InformationCode.EyeLookLeft, "左目です" },
                { InformationCode.NoEyeLookRight, "右目が設定されていません。" },
                { InformationCode.NoEyeLookLeft, "左目が設定されていません。" },
                { InformationCode.EyeLookOutOfRange, "目がアバターの範囲外にあります。" },
                { InformationCode.EyeLookRightOutOfRange, "右目がアバターの範囲外にあります。" },
                { InformationCode.EyeLookLeftOutOfRange, "左目がアバターの範囲外にあります。" },
                { InformationCode.HasntVRCAvatarDescriptor, "VRC Avatar Descriptorがありません。" },
                { InformationCode.HasMultipleVRCAvatarDescriptor,"VRC Avatar Descriptorが複数あります。\n他にも同じエラーが出ていると思いますので、不要なものを消してください"},
                { InformationCode.HasAnimatorController, "AnimatorにControllerが設定されています。\nPlay時にポーズが変更される恐れがあります。" },
                { InformationCode.HasntVRCPipelineManager, "Pipeline Managerがありません。\nアップロード時に自動的に追加されます。" },
                { InformationCode.HasMultipleVRCPipelineManager, "Pipeline Managerが複数個設定されています。\n他にも同じエラーが出ていると思いますので、不要なものを消してください\"" },
                { InformationCode.PhysboneRadiusIsZero, "Physboneの当たり判定が0になっています。" },
                { InformationCode.PhysboneCollisionDisabled, "Physboneの衝突判定が無効になっています。" },
                { InformationCode.PhysboneHasNullCollder, "Physboneコライダーが設定されていない項目があります。" },
                { InformationCode.BoneEnd, "Boneの終端です。\nメッシュに参照されていないと思われますが、3Dモデリングソフトの仕様です。\nその場合自己判断で削除しても問題ありません。" },
                { InformationCode.HasntSkinnedMesh, "メッシュがありません。" },
                { InformationCode.SkinnedMeshRendererHasntBone,"メッシュのボーンが存在しません"},
                { InformationCode.IsEditorOnly,"タグがEditorOnly\n(アップロード時には使わない)になっています"},
                { InformationCode.IsProbeAnchor,"ProbeAnchor(明るさ計算の基点)です" },
                { InformationCode.ProbeAnchorOutOfRange,"ProbeAnchor(明るさ計算の基点)がアバター外にあります" }
            };

            UITranslate = new Dictionary<UICode, string>
            {
                {UICode.Polygon, "ポリゴン" },
                {UICode.Polygons,"ポリゴン" },
                {UICode.Name,"名前" },
                {UICode.Component,"コンポーネント" },
                {UICode.Information,"情報" },
                {UICode.ObjectOutOfRange,"アバター外のオブジェクトが選択されています"},
                {UICode.PleaseSelectObject,"オブジェクトを選択してください"},
                {UICode.SelectedMultipleObjects,"複数のオブジェクトが選択されています"},
                {UICode.MissngScriptRemoveBTN, "Missing Script を削除する" },
                {UICode.ProbeAnchorOveriderBTN,"全体のProbeAnchor(明るさ計算の基点)を書き換える" },
                {UICode.AnchorTransformNotFoundInSelected,"アンカーがアバター内に見つかりませんでした" },
                {UICode.HasMultipleMethod,"複数の解決策があります" },
                {UICode.Method,"方法" },
                {UICode.Delete,"削除" },
                {UICode.EffectComponent,"影響されているコンポーネント" },
                {UICode.ContainPrefab,"プレハブが含まれています\nオブジェクトの自動修正などに影響が出ます\nUnpack推奨です" },
            };

            FUITranslate = new Dictionary<FixUICode, string> {
                {FixUICode.TryReplace,"置き換えを試行する" },
                {FixUICode.TryReplaceAll,"全てで置き換えを試行する" },
                {FixUICode.DeleteComponent,"コンポーネントを削除する" },
                {FixUICode.DeleteOutOfRange,"範囲外をすべて削除する" },
                {FixUICode.DeleteNull,"Null(未選択)を削除する" },
                {FixUICode.DeleteController,"コントローラーを削除する" },
                {FixUICode.SetRadius,"半径をセットする" },
                {FixUICode.NotFound,"見つかりませんでした" },
                {FixUICode.Result,"結果" },
                {FixUICode.Success,"成功しました" },
                {FixUICode.AddAvatarDescripter,"Avatar Descripterを追加する" },
            };
        }
        public static string GetTra(InformationCode IC)
        {
            if (InformationCodeTranslate.ContainsKey(IC))
                return InformationCodeTranslate[IC];
            return "Missing Translate : " + IC == null ? "Null" : IC.ToString();
        }
        public static string GetTra(UICode UIC)
        {
            if (UITranslate.ContainsKey(UIC))
                return UITranslate[UIC];
            return "Missing Translate : " + UIC == null ? "Null" : UIC.ToString();
        }
        public static string GetTra(FixUICode FUC)
        {
            if (FUITranslate.ContainsKey(FUC))
                return FUITranslate[FUC];
            return "Missing Translate : " + FUC == null ? "Null" : FUC.ToString();
        }
    }
}