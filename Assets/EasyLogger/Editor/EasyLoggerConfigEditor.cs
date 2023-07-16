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

            EditorGUILayout.PropertyField(selected.FindPropertyRelative(nameof(ConfigEntry.receiveUnityLogEvents)));
            EditorGUILayout.PropertyField(selected.FindPropertyRelative(nameof(ConfigEntry.filter)));

            SerializedProperty imGuiAppender = selected.FindPropertyRelative(nameof(ConfigEntry.imGuiAppender));
            EditorGUILayout.PropertyField(imGuiAppender);
            if (imGuiAppender.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(selected.FindPropertyRelative(nameof(ConfigEntry.imGuiSwitcherAlignment)));
                EditorGUI.indentLevel--;
            }

            SerializedProperty fileAppender = selected.FindPropertyRelative(nameof(ConfigEntry.fileAppender));
            EditorGUILayout.PropertyField(fileAppender);
            if (fileAppender.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(selected.FindPropertyRelative(nameof(ConfigEntry.maxFileCountKept)));
                EditorGUILayout.PropertyField(selected.FindPropertyRelative(nameof(ConfigEntry.maxDaysKept)));
                EditorGUI.indentLevel--;
            }

            SerializedProperty unityConsoleAppender = selected.FindPropertyRelative(nameof(ConfigEntry.unityConsoleAppender));
            EditorGUILayout.PropertyField(unityConsoleAppender);
            if (unityConsoleAppender.boolValue)
            {
                // EditorGUI.indentLevel++;
                // EditorGUI.indentLevel--;
            }

            SerializedProperty wsServerAppender = selected.FindPropertyRelative(nameof(ConfigEntry.wsServerAppender));
            EditorGUILayout.PropertyField(wsServerAppender);
            if (wsServerAppender.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(selected.FindPropertyRelative(nameof(ConfigEntry.remoteUri)));
                EditorGUI.indentLevel--;
            }

            // customLoggerEntries
            SerializedProperty customLoggerEntries = selected.FindPropertyRelative(nameof(ConfigEntry.customLoggerEntries));
            EditorGUILayout.PropertyField(customLoggerEntries);

            serializedObject.ApplyModifiedProperties();
        }

        [MenuItem("AillieoUtils/EasyLogger/Settings")]
        private static void OpenProjectSettings()
        {
            SettingsService.OpenProjectSettings(EasyLoggerConfig.settingsPath);
        }
    }
}
