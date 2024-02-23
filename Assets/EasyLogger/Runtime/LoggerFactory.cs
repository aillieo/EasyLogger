// -----------------------------------------------------------------------
// <copyright file="LoggerFactory.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyLogger
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class LoggerFactory
    {
        private static readonly Dictionary<string, Logger> cachedInstances = new Dictionary<string, Logger>(StringComparer.Ordinal);
        private static readonly Dictionary<string, CustomLoggerEntry> customLoggerEntries = new Dictionary<string, CustomLoggerEntry>(StringComparer.Ordinal);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Init()
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

            if (config.wsServerAppender)
            {
                try
                {
                    Uri uri = new Uri(config.remoteUri);
                    WSServerAppender wsServerAppender = new WSServerAppender(uri);
                    Logger.sharedAppenders.Add(wsServerAppender);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }

            Logger.receiveUnityLogEvents = config.receiveUnityLogEvents;

            if (config.customLoggerEntries != null)
            {
                foreach (var custom in config.customLoggerEntries)
                {
                    customLoggerEntries[custom.module] = custom;
                }
            }
        }

        public static Logger GetLogger(string moduleName)
        {
            if (!cachedInstances.TryGetValue(moduleName, out Logger instance))
            {
                instance = new Logger(moduleName);
                cachedInstances.Add(moduleName, instance);

                if (customLoggerEntries.TryGetValue(moduleName, out CustomLoggerEntry custom))
                {
                    instance.filter = custom.filter;
                }
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
