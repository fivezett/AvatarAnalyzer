using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AvatarAnalyzer
{
    public class EZ_UIDrawer
    {
        public static bool BTN(FixUICode FUC, bool Bold = false)
        {
            return BTN(Language.GetTra(FUC), Bold);
        }

        public static bool BTN(UICode UIC, bool Bold = false)
        {
            return BTN(Language.GetTra(UIC), Bold);
        }

        public static bool BTN(string text, bool Bold = false)
        {
            if (Bold)
                return GUILayout.Button(text, EditorStyles.boldLabel);
            else
                return GUILayout.Button(text);
        }

        public static void Label(UICode UIC, bool Bold = false)
        {
            Label(Language.GetTra(UIC), Bold);
        }

        public static void Label(string text, bool Bold = false)
        {
            if (Bold)
                GUILayout.Label(text, EditorStyles.boldLabel);
            else
                GUILayout.Label(text);
        }

        public static void WriteLine(float height)
        {
            WriteLine(height, Color.white);
        }

        public static void WriteLine(float line1, float height, float line2)
        {
            EditorGUILayout.Space(line1);
            WriteLine(height, Color.white);
            EditorGUILayout.Space(line2);
        }

        public static void WriteLine(float height, Color color)
        {
            var colorBackup = GUI.backgroundColor;
            GUI.backgroundColor = color;
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(height));
            GUI.backgroundColor = colorBackup;
        }

        private static Color BG_Origin = GUI.backgroundColor;
        private static Color[] BG_Table = new Color[] { Color.green, Color.cyan };
        private static int BG_Index = 0;
        public static void SwitchBG()
        {
            GUI.backgroundColor = BG_Table[BG_Index++];
            if (BG_Index == BG_Table.Length) BG_Index = 0;
        }
        public static void ResetBG()
        {
            GUI.backgroundColor = BG_Origin;
        }
    }
}