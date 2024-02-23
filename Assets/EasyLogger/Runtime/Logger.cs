// -----------------------------------------------------------------------
// <copyright file="Logger.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyLogger
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using UnityEngine;

    public class Logger
    {
        internal static readonly List<IAppender> sharedAppenders = new List<IAppender>();

        internal static LogLevel sharedFilter = LogLevel.Any;

        private static readonly IFormatter defaultFormatter = new DefaultFormatter();

        private static bool isReceivingUnityLogEvents = false;

        private readonly string moduleName;

        private LogLevel? instanceFilter = null;

        private List<IAppender> appenders = null;

        internal Logger(string moduleName)
        {
            this.moduleName = moduleName;
        }

        public static bool receiveUnityLogEvents
        {
            get
            {
                return isReceivingUnityLogEvents;
            }

            set
            {
                if (isReceivingUnityLogEvents != value)
                {
                    isReceivingUnityLogEvents = value;
                    if (isReceivingUnityLogEvents)
                    {
                        Application.logMessageReceivedThreaded -= OnUnityLogEvent;
                        Application.logMessageReceivedThreaded += OnUnityLogEvent;
                        Logger logger = LoggerFactory.GetLogger<UnityEngine.Debug>();
                        logger.RemoveAppender<UnityConsoleAppender>();
                    }
                    else
                    {
                        Application.logMessageReceivedThreaded -= OnUnityLogEvent;
                    }
                }
            }
        }

        public LogLevel filter
        {
            get
            {
                if (this.instanceFilter.HasValue)
                {
                    return this.instanceFilter.Value;
                }

                return sharedFilter;
            }

            set
            {
                this.instanceFilter = value;
            }
        }

        public void AddAppender(IAppender appender)
        {
            if (this.appenders == null)
            {
                this.appenders = new List<IAppender>(sharedAppenders);
            }

            this.appenders.Add(appender);
        }

        public void AddAppenders(params IAppender[] appenders)
        {
            if (this.appenders == null)
            {
                this.appenders = new List<IAppender>(sharedAppenders);
            }

            this.appenders.AddRange(appenders);
        }

        public void RemoveAppender(IAppender appender)
        {
            if (this.appenders == null)
            {
                this.appenders = new List<IAppender>(sharedAppenders);
            }

            this.appenders.Remove(appender);
        }

        public void RemoveAppender<T>()
            where T : IAppender
        {
            if (this.appenders == null)
            {
                this.appenders = new List<IAppender>(sharedAppenders);
            }

            this.appenders.RemoveAll(a => a is T);
        }

        public void RemoveAppender(Type type)
        {
            if (this.appenders == null)
            {
                this.appenders = new List<IAppender>(sharedAppenders);
            }

            this.appenders.RemoveAll(a => type.IsAssignableFrom(a.GetType()));
        }

        public void RemoveAllAppenders()
        {
            if (this.appenders == null)
            {
                this.appenders = new List<IAppender>();
            }
        }

        public void Debug(object message)
        {
            this.LogWithFilter(LogLevel.Debug, message);
        }

        public void Log(object message)
        {
            this.LogWithFilter(LogLevel.Log, message);
        }

        public void Warning(object message)
        {
            this.LogWithFilter(LogLevel.Warning, message);
        }

        public void Error(object message)
        {
            this.LogWithFilter(LogLevel.Error, message);
        }

        private static int internalCall = 0;

        private void LogWithFilter(LogLevel logLevel, object message)
        {
            if (internalCall > 0)
            {
                return;
            }

            internalCall++;

            if ((this.filter & logLevel) > 0)
            {
                DateTime dateTime = DateTime.Now;
                int threadId = Thread.CurrentThread.ManagedThreadId;
                string stackTrace = (logLevel & LogLevel.Error) > 0 ?
                    StackTraceHelper.Extract(3) : string.Empty;
                foreach (var appender in this.appenders ?? sharedAppenders)
                {
                    try
                    {
                        IFormatter formatter = appender.formatter ?? defaultFormatter;
                        var msg = formatter.Format(
                            this.moduleName,
                            logLevel,
                            message,
                            dateTime,
                            threadId,
                            stackTrace);
                        LogItem logItem = new LogItem(this.moduleName, logLevel, msg, null);
                        appender.OnReceiveLogItem(ref logItem);
                    }
                    catch
                    {
                    }
                }
            }

            internalCall--;
        }

        private static void OnUnityLogEvent(string condition, string stackTrace, LogType type)
        {
            Logger logger = LoggerFactory.GetLogger<UnityEngine.Debug>();

            switch (type)
            {
                case LogType.Warning:
                    logger.Warning(condition);
                    break;
                case LogType.Error:
                case LogType.Assert:
                case LogType.Exception:
                    logger.Error(condition);
                    break;
                default:
                    logger.Log(condition);
                    break;
            }
        }
    }
}
