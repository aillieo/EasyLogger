using System;
using System.Collections.Generic;
using System.Net;

namespace AillieoUtils.EasyLogger
{
    public static class LoggerFactory
    {
        private static readonly Dictionary<string, Logger> cachedInstances = new Dictionary<string, Logger>(StringComparer.Ordinal);

        public static void Init()
        {
            ConfigEntry config = EasyLoggerConfig.GetConfig();

            Logger.sharedFilter = config.filter;
            if (config.imGuiAppender)
            {
                IMGUIAppender imGuiAppender = new IMGUIAppender();
                imGuiAppender.switcherAlignment = config.imGuiSwitcherAlignment;
                Logger.sharedAppenders.Add(imGuiAppender);
            }

            if (config.fileAppender)
            {
                FileAppender fileAppender = new FileAppender(config.maxFileCountKept, config.maxDaysKept);
                Logger.sharedAppenders.Add(fileAppender);
            }

            if (config.unityConsoleAppender)
            {
                UnityConsoleAppender unityConsoleAppender = new UnityConsoleAppender();
                Logger.sharedAppenders.Add(unityConsoleAppender);
            }

            Logger.receiveUnityLogEvents = config.receiveUnityLogEvents;
        }

        public static Logger GetLogger(string moduleName)
        {
            if (!cachedInstances.TryGetValue(moduleName, out Logger instance))
            {
                instance = new Logger(moduleName);
                cachedInstances.Add(moduleName, instance);
            }

            return instance;
        }

        public static Logger GetLogger(Type type)
        {
            return GetLogger(type.FullName);
        }

        public static Logger GetLogger<T>()
        {
            return GetLogger(typeof(T));
        }
    }
}
