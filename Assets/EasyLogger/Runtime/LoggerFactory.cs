using System;
using System.Collections.Generic;

namespace AillieoUtils.EasyLogger
{
    public static class LoggerFactory
    {
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

        private static readonly Dictionary<string, Logger> cachedInstances = new Dictionary<string, Logger>();

        public static Logger GetLogger(string name)
        {
            if (!cachedInstances.TryGetValue(name, out Logger instance))
            {
                instance = new Logger(name);
                cachedInstances.Add(name, instance);
            }

            return instance;
        }

        public static Logger GetLogger<T>()
        {
            return GetLogger(typeof(T).FullName);
        }
    }
}
