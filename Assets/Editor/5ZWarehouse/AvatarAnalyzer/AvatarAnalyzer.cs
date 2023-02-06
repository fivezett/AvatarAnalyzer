using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System;

namespace AvatarAnalyzer
{
    public class AvatarAnalyzerCore : EditorWindow
    {
        private GameObject lockSelect;
        private Animator avatarAnimator;
        private ObjectItemMG objectItemMG = new ObjectItemMG();
        private Vector2 _scrollPosition = Vector2.zero;
        private readonly ProbeAnchorOverider probeAnchorOverider = new ProbeAnchorOverider();
        public Dictionary<InformationCode, List<IFixFunction>> fixFunctions = new FixFunctions.FixFunctionsList().Get();
        public Dictionary<string, bool> overviewFold = new Dictionary<string, bool>{
                {"SkinnedMeshRenderer", false},
            { "MeshFilter",false},
            { "Material",false},
            };
        private readonly string[] tabName = new string[] { "Overview", "Detail", "Edit", /*"Settings"*/ };
        private readonly string[] tabName2 = new string[] { "WarnFix", "BadFix", "EmptyDelete", "Others" };
        private int tabIndex = 0, tabIndex2 = 0;

        [MenuItem("Tools/AvatarAnalyzer")]
        public static void ShowWindow()
        {
            var window = EditorWindow.GetWindow(typeof(AvatarAnalyzerCore), false, "AvatarAnalyzer v0.4.1β");
        }

        private int UpdateTimer = 0;

        private void UpdateNextFrame()
        {
            UpdateTimer = 300;
        }

        private void Update()
        {
            if (++UpdateTimer >= 300)
            {
                UpdateTimer = 0;
                objectItemMG.Set(avatarAnimator);
                Repaint();
            }
        }
        void OnGUI()
        {
            Draw();
        }
        void Draw()
        {
            avatarAnimator = EditorGUILayout.ObjectField("アバター", avatarAnimator, typeof(Animator), true) as Animator;
            if (avatarAnimator != null)
            {
                if (objectItemMG.Prefabs != null && objectItemMG.Prefabs.Any())
                {
                    EditorGUILayout.HelpBox(Language.GetTra(UICode.ContainPrefab), MessageType.Warning);
                    using (new EditorGUI.DisabledGroupScope(true))
                        objectItemMG.Prefabs.ToList().ForEach(GO => EditorGUILayout.ObjectField(GO, typeof(GameObject), true));
                }

                tabName2[0] = "WarnFix(" + objectItemMG.GetWarnAttributeSize().ToString() + ")";
                tabName2[1] = "BadFix(" + objectItemMG.GetBadAttributeSize().ToString() + ")";
                tabName2[2] = "EmptyDelete(" + objectItemMG.GetDeleteAttributeSize().ToString() + ")";

                tabIndex = GUILayout.Toolbar(tabIndex, tabName);


                if (tabIndex == 0)
                {
                    _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                    DrawOverview();
                }
                else if (tabIndex == 1)
                {
                    _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                    DrawDetail();
                }
                else if (tabIndex == 2)
                {
                    tabIndex2 = GUILayout.Toolbar(tabIndex2, tabName2);
                    _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                    if (tabIndex2 == 0)
                        DrawInfoAll(InfoType.Warn, MessageType.Warning);
                    else if (tabIndex2 == 1)
                        DrawInfoAll(InfoType.Bad, MessageType.Error);
                    else if (tabIndex2 == 2)
                        DrawEmptyDelete();
                    else if (tabIndex2 == 3)
                    {
                        EZ_UIDrawer.Label("MissingScriptRemove", true);
                        if (EZ_UIDrawer.BTN(Language.GetTra(UICode.MissngScriptRemoveBTN)))
                            MissingScriptRemoveFunction.Run(objectItemMG, true);
                        EZ_UIDrawer.WriteLine(3, 1, 3);
                        EZ_UIDrawer.Label("EZ ProbeAnchorOverider", true);
                        probeAnchorOverider.Anchor = EditorGUILayout.ObjectField("ProbeAnchor", probeAnchorOverider.Anchor, typeof(GameObject), true) as GameObject;
                        if (EZ_UIDrawer.BTN(Language.GetTra(UICode.ProbeAnchorOveriderBTN)))
                            probeAnchorOverider.Run(objectItemMG, true);
                    }
                }
                else if (tabIndex == 3)
                {
                    _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                    EZ_UIDrawer.Label("マダナイヨ");
                }
                EditorGUILayout.EndScrollView();
            }
        }

        private void DrawOverview()
        {
            var skinMeshRenderers = objectItemMG.GetHasComponentObjects<SkinnedMeshRenderer>();
            var meshFilters = objectItemMG.GetHasComponentObjects<MeshFilter>();
            HashSet<Material> Materials = new HashSet<Material>();
            HashSet<Mesh> Meshs = new HashSet<Mesh>();
            HashSet<Mesh> SkinnedMeshs = new HashSet<Mesh>();
            meshFilters.Select(OI => OI.getComponent<MeshFilter>()).ToList().Where(MF => MF != null && MF.sharedMesh != null)
                .ToList().ForEach(MF => Meshs.Add(MF.sharedMesh));
            skinMeshRenderers.Select(OI => OI.getComponent<SkinnedMeshRenderer>()).ToList().Where(SMR => SMR != null && SMR.sharedMesh != null)
                .ToList().ForEach(SMR =>
                {
                    SkinnedMeshs.Add(SMR.sharedMesh);
                    SMR.sharedMaterials.Where(M => M != null).ToList().ForEach(M => Materials.Add(M));
                });

            int meshPolygon = Meshs.Sum(x => x.triangles.Length / 3);
            int SkinnedMeshPolygon = SkinnedMeshs.Sum(x => x.triangles.Length / 3);
            EditorGUILayout.LabelField(Language.GetTra(UICode.Polygon) + " : " + (meshPolygon + SkinnedMeshPolygon).ToString());
            overviewFold["SkinnedMeshRenderer"] = EditorGUILayout.Foldout(overviewFold["SkinnedMeshRenderer"], "SkinnedMeshRenderer(" + skinMeshRenderers.Count().ToString() + ")");
            if (overviewFold["SkinnedMeshRenderer"])
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField(Language.GetTra(UICode.Polygon) + " : " + SkinnedMeshPolygon.ToString());

                skinMeshRenderers.Select(OI => OI.getComponent<SkinnedMeshRenderer>()).ToList().ForEach(SMR =>
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        using (new EditorGUI.DisabledScope(true))
                            EditorGUILayout.ObjectField(SMR, typeof(SkinnedMeshRenderer), true);
                        if (SMR.sharedMesh != null)
                            EditorGUILayout.LabelField((SMR.sharedMesh.triangles.Length / 3).ToString() + " " + Language.GetTra(UICode.Polygons));
                        using (new EditorGUILayout.VerticalScope())
                        {
                            for (int i = 0; i < SMR.sharedMaterials.Length; i++)
                            {
                                Material newMat = EditorGUILayout.ObjectField(SMR.sharedMaterials[i], typeof(Material), true) as Material;
                                if (newMat != SMR.sharedMaterials[i])
                                {
                                    var newMatArr = new List<Material>(SMR.sharedMaterials).ToArray();
                                    newMatArr[i] = newMat;
                                    SMR.sharedMaterials = newMatArr;
                                }
                            }
                        }
                    }
                    EZ_UIDrawer.WriteLine(1);
                });
                EditorGUI.indentLevel--;
            }

            overviewFold["MeshFilter"] = EditorGUILayout.Foldout(overviewFold["MeshFilter"], "MeshFilter(" + meshFilters.Count().ToString() + ")");
            if (overviewFold["MeshFilter"])
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField(Language.GetTra(UICode.Polygon) + " : " + meshPolygon.ToString());

                meshFilters.Select(OI => OI.getComponent<MeshFilter>()).ToList().ForEach(MF =>
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        using (new EditorGUI.DisabledScope(true))
                            EditorGUILayout.ObjectField(MF, typeof(MeshFilter), true);
                        if (MF.sharedMesh != null)
                            EditorGUILayout.LabelField((MF.sharedMesh.triangles.Length / 3).ToString() + " " + Language.GetTra(UICode.Polygons));
                        if (MF.gameObject.GetComponent<MeshRenderer>() != null)
                            using (new EditorGUILayout.VerticalScope())
                            {
                                var MR = MF.gameObject.GetComponent<MeshRenderer>();
                                if (MR != null)
                                    for (int i = 0; i < MR.sharedMaterials.Length; i++)
                                    {
                                        Material newMat = EditorGUILayout.ObjectField(MR.sharedMaterials[i], typeof(Material), true) as Material;
                                        if (newMat != MR.sharedMaterials[i])
                                        {
                                            var newMatArr = new List<Material>(MR.sharedMaterials).ToArray();
                                            newMatArr[i] = newMat;
                                            MR.sharedMaterials = newMatArr;
                                        }
                                    }
                            }
                    }
                    EZ_UIDrawer.WriteLine(1);
                });
                EditorGUI.indentLevel--;
            }

            overviewFold["Material"] = EditorGUILayout.Foldout(overviewFold["Material"], "Material(" + Materials.Count.ToString() + ")");
            if (overviewFold["Material"])
            {
                EditorGUI.indentLevel++;

                Materials.ToList().ForEach(mat =>
                {
                    Material newMAT = EditorGUILayout.ObjectField(mat, typeof(Material), true) as Material;
                    if (newMAT != mat)
                        MaterialReplace.Run(objectItemMG, mat, newMAT);
                    EZ_UIDrawer.WriteLine(1);
                });
                EditorGUI.indentLevel--;
            }
        }

        private class TreeItem
        {
            public TreeItem(ObjectItem Item)
            {
                this.Item = Item;
            }
            public List<TreeItem> Child = new List<TreeItem>();
            public ObjectItem Item;
        }

        private void DrawEmptyDelete()
        {
            var Objectlist = objectItemMG.ObjectList.Values.ToList()
                .Where(OI => !OI.hasDepend()).ToDictionary(OI => OI.obj, OI => new TreeItem(OI));
            List<TreeItem> NL = new List<TreeItem>();
            Objectlist.ToList().ForEach(KV =>
            {
                if (Objectlist.Keys.Contains(KV.Value.Item.obj.transform.parent.gameObject))
                    Objectlist[KV.Value.Item.obj.transform.parent.gameObject].Child.Add(KV.Value);
                else
                    NL.Add(KV.Value);
            });

            Action<TreeItem> ShowTree = null;
            ShowTree = TI =>
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    using (new EditorGUI.DisabledScope(true))
                        EditorGUILayout.ObjectField(TI.Item.obj, typeof(GameObject), true);

                    if (EZ_UIDrawer.BTN(UICode.Delete))
                    {
                        DestroyImmediate(TI.Item.obj);
                        UpdateNextFrame();
                    }
                }
                TI.Child.ForEach(TIc =>
                    {
                        EditorGUI.indentLevel++;
                        ShowTree(TIc);
                        EditorGUI.indentLevel--;
                    });
            };
            NL.ForEach(TI =>
            {
                ShowTree(TI);
                EditorGUILayout.Space(10);
            });
        }

        private void DrawInfoAll(InfoType IT, MessageType MT)
        {
            //FIX IT
            objectItemMG.ObjectList.Values.ToList().Where(OI =>
            {
                if (IT == InfoType.Warn)
                    return OI.warnAttribute.Any();
                else if (IT == InfoType.Bad)
                    return OI.badAttribute.Any();
                return false;
            }).ToList().ForEach(OI =>
            {
                using (new EditorGUI.DisabledScope(true))
                    EditorGUILayout.ObjectField(OI.obj, typeof(GameObject), true);
                DrawInfoOnceOI(OI, IT, MT);

                EZ_UIDrawer.WriteLine(10, 2.5f, 5);
            });
        }

        private void DrawDetail()
        {
            if (Selection.gameObjects.Length == 1)
            {
                if (!objectItemMG.Has(Selection.gameObjects[0]))
                    EditorGUILayout.HelpBox(Language.GetTra(UICode.ObjectOutOfRange), MessageType.Warning);
                else
                {
                    if (GUILayout.Button(lockSelect == null ? "Lock" : "UnLock"))
                        lockSelect = lockSelect == null ? Selection.gameObjects[0] : null;
                    DrawCore(lockSelect == null ? Selection.gameObjects[0] : lockSelect);
                }
            }
            else
            {
                if (Selection.gameObjects.Length == 0)
                    EditorGUILayout.HelpBox(Language.GetTra(UICode.PleaseSelectObject), MessageType.Warning);
                else
                    EditorGUILayout.HelpBox(Language.GetTra(UICode.SelectedMultipleObjects), MessageType.Warning);
            }
        }

        private void DrawCore(GameObject target)
        {
            var objectItem = objectItemMG.Get(target);
            EditorGUILayout.LabelField(Language.GetTra(UICode.Name) + " : " + objectItem.obj.name);

            if (objectItem.component.Count != 0)
                EditorGUILayout.LabelField(Language.GetTra(UICode.Component) + " : ");

            EditorGUI.indentLevel++;
            foreach (var compKV in objectItem.component)
                EditorGUILayout.ObjectField(compKV.Key, compKV.Value, typeof(Component), true);
            EditorGUI.indentLevel--;

            EditorGUILayout.LabelField(Language.GetTra(UICode.Information) + " : ");
            if (!objectItem.hasDepend())
                EditorGUILayout.HelpBox(Language.GetTra(InformationCode.HasntDepend), MessageType.Error);

            DrawInfoOnceOI(objectItem, InfoType.Bad, MessageType.Error);
            DrawInfoOnceOI(objectItem, InfoType.Warn, MessageType.Warning);
            DrawInfoOnceOI(objectItem, InfoType.Normal, MessageType.Info);
        }

        void DrawInfoOnceOI(ObjectItem OI, InfoType IT, MessageType MT)
        {
            EZ_UIDrawer.SwitchBG();

            OI.GetAttribute(IT).ToList().ForEach(atr =>
                {
                    EditorGUILayout.HelpBox(Language.GetTra(atr.Key.Key), MT);

                    //コンポーネント表示
                    if (atr.Key.Value != null)
                        using (new EditorGUI.DisabledScope(true))
                            EditorGUILayout.ObjectField(atr.Key.Value, typeof(Component), true);

                    //影響のあるコンポーネント表示
                    EditorGUI.indentLevel++;
                    if (atr.Value.Any())
                        EditorGUILayout.LabelField(Language.GetTra(UICode.EffectComponent) + " : ");
                    using (new EditorGUI.DisabledScope(true))
                        atr.Value.ForEach(item => EditorGUILayout.ObjectField(item, typeof(Component), true));
                    EditorGUI.indentLevel--;

                    if (fixFunctions.ContainsKey(atr.Key.Key))
                    {
                        bool isMultiple = fixFunctions[atr.Key.Key].Skip(1).Any();
                        if (isMultiple)
                        {
                            GUILayout.Space(5);
                            EditorGUILayout.HelpBox(Language.GetTra(UICode.HasMultipleMethod), MessageType.Info);
                        }
                        int i = 0;
                        foreach (IFixFunction func in fixFunctions[atr.Key.Key])
                        {
                            if (isMultiple)
                                EZ_UIDrawer.Label(Language.GetTra(UICode.Method) + " " + ++i);
                            if (func.Draw(objectItemMG, OI, atr.Key.Value, atr.Value))
                                UpdateNextFrame();
                            if (isMultiple)
                                GUILayout.Space(10);
                        }
                    }

                });
            EZ_UIDrawer.ResetBG();

        }

        private void OnSelectionChange()
        {
            Repaint();
        }
    }
}