using System;
using UnityEngine;

namespace AillieoUtils.EasyLogger
{
    [Serializable]
    public class ConfigEntry
    {
        public bool receiveUnityLogEvents;
        public LogLevel filter;

        public bool imGuiAppender;
        public IMGUIAppender.Alignment imGuiSwitcherAlignment = IMGUIAppender.Alignment.TopLeft;

        public bool fileAppender;
        public int maxFileCountKept = 100;
        public int maxDaysKept = 30;

        public bool unityConsoleAppender;

        public bool wsServerAppender;
        public string remoteUri = "ws://localhost:8080";
    }

    [SettingsMenuPath(settingsPath)]
    public class EasyLoggerConfig : SingletonScriptableObject<EasyLoggerConfig>
    {
        public const string settingsPath = "AillieoUtils/EasyLogger";

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
#elif DEVELOPMENT_BUILD
            configEntry = Instance.debugConfig;
#else
            configEntry = Instance.releaseConfig;
#endif

            return configEntry;
        }

        protected override void Awake()
        {
            base.Awake();

            if (Application.isPlaying)
            {
                LoggerFactory.Init();
            }
        }
    }
}
