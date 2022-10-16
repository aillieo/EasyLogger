using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyLogger
{
    [CustomEditor(typeof(EasyLoggerConfig))]
    internal class EasyLoggerConfigEditor : Editor
    {
        private static readonly string[] presets = new string[]
        {
            "Editor", "Debug", "Release",
        };

        private int tab = 0;

        public override void OnInspectorGUI()
        {
            tab = GUILayout.Toolbar(tab, presets);
            SerializedProperty selected = null;
            switch (tab)
            {
                case 0:
                    selected = serializedObject.FindProperty("editorConfig");
                    break;
                case 1:
                    selected = serializedObject.FindProperty("debugConfig");
                    break;
                case 2:
                    selected = serializedObject.FindProperty("releaseConfig");
                    break;
                default:
                    throw new IndexOutOfRangeException();
            }

            IEnumerator enumerator = selected.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current is SerializedProperty prop)
                {
                    EditorGUILayout.PropertyField(prop);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
