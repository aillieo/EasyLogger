using System;
using System.Collections.Generic;

namespace AillieoUtils.EasyLogger
{
    public static class LoggerFactory
    {
        private static readonly Dictionary<string, Logger> cachedInstances = new Dictionary<string, Logger>(StringComparer.Ordinal);

        static LoggerFactory()
        {
            ConfigEntry config = EasyLoggerConfig.GetConfig();

            Logger.sharedFilter = config.filter;
            if (config.imGuiAppender)
            {
                Logger.sharedAppenders.Add(new IMGUIAppender());
            }

            if (config.fileAppender)
            {
                Logger.sharedAppenders.Add(new FileAppender());
            }

            if (config.unityConsoleAppender)
            {
                Logger.sharedAppenders.Add(new UnityConsoleAppender());
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
