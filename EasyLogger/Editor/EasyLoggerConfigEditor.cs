using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyLogger
{
    internal class EasyLoggerConfigEditor
    {
        [SettingsProvider]
        private static SettingsProvider CreateSettingsProvider()
        {
            return new SettingsProvider("AillieoUtils/EasyLogger", SettingsScope.Project)
            {
                label = "EasyLogger",
                guiHandler = OnGUI,
                keywords = new HashSet<string>(new[] { "Aillieo", "Log", }),
            };
        }

        private static void OnGUI(string searchContext)
        {
            EasyLoggerConfig.GetConfig();

            EasyLoggerConfig config = EasyLoggerConfig.GetConfig();
            SerializedObject serializedObject = new SerializedObject(config);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("receiveUnityLogEvents"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("filter"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("imGuiAppender"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fileAppender"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("unityConsoleAppender"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
