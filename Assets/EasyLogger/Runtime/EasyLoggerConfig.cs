using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AillieoUtils.EasyLogger
{
    [Serializable]
    public class EasyLoggerConfig : ScriptableObject
    {
        public bool receiveUnityLogEvents;
        public LogLevel filter;
        public bool imGuiAppender;
        public bool fileAppender;
        public bool unityConsoleAppender;

        private static EasyLoggerConfig globalConfig;
        private static readonly string defaultPath = "Assets/AillieoUtils/EasyLogger/Config.asset";

#if UNITY_EDITOR
        private static void GetOrCreateAsset()
        {
            EasyLoggerConfig config = default;
            string[] guids = AssetDatabase.FindAssets($"t:{nameof(EasyLoggerConfig)}");
            if (guids != null && guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                config = AssetDatabase.LoadAssetAtPath<EasyLoggerConfig>(path);
            }

            if (config == null)
            {
                config = CreateInstance<EasyLoggerConfig>();
                FileInfo fileInfo = new FileInfo(defaultPath);
                Directory.CreateDirectory(fileInfo.Directory.FullName);
                AssetDatabase.CreateAsset(config, defaultPath);
            }

            if (config == null)
            {
                throw new Exception("");
            }

            List<UnityEngine.Object> preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
            if (!preloadedAssets.Contains(config))
            {
                preloadedAssets.Add(config);
                PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
            }
        }
#endif

        public static EasyLoggerConfig GetConfig()
        {
#if UNITY_EDITOR
            if (globalConfig == null)
            {
                GetOrCreateAsset();
            }
#endif
            return globalConfig;
        }

        private void OnEnable()
        {
            globalConfig = this;
        }
    }
}
