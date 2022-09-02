using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyLogger
{
    public static class UnityConsoleLogLocator
    {
        private static readonly string targetScript = "EasyLogger/Runtime/Logger.cs";
        private static readonly int targetScriptInstanceId;
        private static readonly Regex regex = new Regex(@"\<a href=""([\S]+.cs)"" line=""([\d]+)""\>");

        static UnityConsoleLogLocator()
        {
            string targetPath = AssetDatabase.GetAllAssetPaths().FirstOrDefault(p => p.EndsWith(targetScript, StringComparison.InvariantCulture));
            MonoScript scriptObj = AssetDatabase.LoadAssetAtPath<MonoScript>(targetPath);
            targetScriptInstanceId = scriptObj.GetInstanceID();
        }

        [UnityEditor.Callbacks.OnOpenAsset(-1)]
        private static bool OnOpenAsset(int instanceID, int line)
        {
            if (instanceID == targetScriptInstanceId)
            {
                string activeText = GetConsoleActiveText();
                if (string.IsNullOrWhiteSpace(activeText))
                {
                    return false;
                }

                string[] lines = activeText.Split('\n');
                foreach (string oneLine in lines)
                {
                    Match match = regex.Match(oneLine);
                    if (!match.Success)
                    {
                        continue;
                    }

                    if (match.Groups.Count < 3)
                    {
                        continue;
                    }

                    string filename = match.Groups[1].Value;
                    int.TryParse(match.Groups[2].Value, out var fileLine);

                    filename = filename.Replace("\\", "/").Replace(Application.dataPath, "Assets/");

                    UnityEngine.Object uobj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(filename);
                    if (uobj == null)
                    {
                        continue;
                    }

                    AssetDatabase.OpenAsset(uobj, fileLine);
                    return true;
                }
            }

            return false;
        }

        private static string GetConsoleActiveText()
        {
            Type consoleWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");
            FieldInfo fieldInfo = consoleWindowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
            object consoleWindowInstance = fieldInfo.GetValue(null);

            if (consoleWindowInstance != null)
            {
                if ((object)EditorWindow.focusedWindow == consoleWindowInstance)
                {
                    fieldInfo = consoleWindowType.GetField("m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic);
                    string activeText = fieldInfo.GetValue(consoleWindowInstance).ToString();
                    return activeText;
                }
            }

            return string.Empty;
        }
    }
}
