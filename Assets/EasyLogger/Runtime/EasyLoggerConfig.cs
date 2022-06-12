using System;
using UnityEngine;

namespace AillieoUtils.EasyLogger
{
    [Serializable]
    public struct ConfigEntry
    {
        public bool receiveUnityLogEvents;
        public LogLevel filter;
        public bool imGuiAppender;
        public bool fileAppender;
        public bool unityConsoleAppender;
    }

    [SettingsMenuPath("AillieoUtils/EasyLogger")]
    public class EasyLoggerConfig : SingletonScriptableObject<EasyLoggerConfig>
    {
        [SerializeField]
        private ConfigEntry editorConfig = new ConfigEntry()
        {
            receiveUnityLogEvents = true,
            filter = LogLevel.Any,
            imGuiAppender = false,
            fileAppender = false,
            unityConsoleAppender = true,
        };

        [SerializeField]
        private ConfigEntry debugConfig = new ConfigEntry()
        {
            receiveUnityLogEvents = true,
            filter = LogLevel.Any,
            imGuiAppender = true,
            fileAppender = true,
            unityConsoleAppender = false,
        };

        [SerializeField]
        private ConfigEntry releaseConfig = new ConfigEntry()
        {
            receiveUnityLogEvents = true,
            filter = LogLevel.NonDebug,
            imGuiAppender = false,
            fileAppender = true,
            unityConsoleAppender = false,
        };

        public static ConfigEntry GetConfig()
        {
            ConfigEntry configEntry = default;

#if UNITY_EDITOR
            configEntry = Instance.editorConfig;
#elif DEBUG
            configEntry = Instance.debugConfig;
#else
            configEntry = Instance.releaseConfig;
#endif

            return configEntry;
        }
    }
}
