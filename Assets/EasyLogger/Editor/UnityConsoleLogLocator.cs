// -----------------------------------------------------------------------
// <copyright file="UnityConsoleLogLocator.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyLogger
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using UnityEditor;

    internal static class UnityConsoleLogLocator
    {
        private static readonly string loggerScript = "EasyLogger/Runtime/Logger.cs";
        private static readonly int loggerScriptInstanceId;
        private static readonly Regex regex = new Regex(@"\<a href=""([\S]+.cs)"" line=""([\d]+)""\>");
        private static readonly string objectExtensionsScript = "EasyLogger/Runtime/Extensions/ObjectExtensions.cs";
        private static readonly int objectExtensionsScriptInstanceId;
        private static readonly Regex regex0 = new Regex(@"at ([\S]+.cs):([\d]+)");

        static UnityConsoleLogLocator()
        {
            string loggerScriptPath = AssetDatabase.GetAllAssetPaths().FirstOrDefault(p => p.EndsWith(loggerScript, StringComparison.InvariantCulture));
            MonoScript loggerScriptObj = AssetDatabase.LoadAssetAtPath<MonoScript>(loggerScriptPath);
            loggerScriptInstanceId = loggerScriptObj.GetInstanceID();

            string objectExtensionsScriptPath = AssetDatabase.GetAllAssetPaths().FirstOrDefault(p => p.EndsWith(objectExtensionsScript, StringComparison.InvariantCulture));
            MonoScript objectExtensionsScriptObj = AssetDatabase.LoadAssetAtPath<MonoScript>(objectExtensionsScriptPath);
            objectExtensionsScriptInstanceId = objectExtensionsScriptObj.GetInstanceID();
        }

        [UnityEditor.Callbacks.OnOpenAsset(-1)]
        private static bool OnOpenAsset(int instanceID, int line)
        {
            if (instanceID == loggerScriptInstanceId)
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

                    UnityEngine.Object uobj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(filename);
                    if (uobj == null)
                    {
                        continue;
                    }

                    AssetDatabase.OpenAsset(uobj, fileLine);
                    return true;
                }
            }
            else if (instanceID == objectExtensionsScriptInstanceId)
            {
                string activeText = GetConsoleActiveText();
                if (string.IsNullOrWhiteSpace(activeText))
                {
                    return false;
                }

                string[] lines = activeText.Split('\n');
                foreach (string oneLine in lines)
                {
                    Match match = regex0.Match(oneLine);
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
