using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyLogger
{
    internal class EasyLoggerConfig
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
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField("EasyLogger:");



            EditorGUILayout.EndVertical();
        }

        private static string GetConfigPath()
        {
            return Path.Combine(UnityEngine.Application.dataPath, "EasyLogger.json");
        }
    }
}
