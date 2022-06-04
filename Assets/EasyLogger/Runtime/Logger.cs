using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace AillieoUtils.EasyLogger
{
    public class Logger
    {
        internal Logger(string name)
        {
            this.name = name;
        }

        private readonly string name;

        private static readonly IFormatter defaultFormatter = new DefaultFormatter();

        private static bool isReceivingUnityLogEvents = false;
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
                        Application.logMessageReceived -= OnUnityLogEvent;
                        Application.logMessageReceived += OnUnityLogEvent;
                    }
                    else
                    {
                        Application.logMessageReceived -= OnUnityLogEvent;
                    }
                }
            }
        }

        internal static LogLevel sharedFilter = LogLevel.Any;
        private LogLevel? instanceFilter = null;

        public LogLevel filter
        {
            get
            {
                if (instanceFilter.HasValue)
                {
                    return instanceFilter.Value;
                }

                return sharedFilter;
            }

            set
            {
                instanceFilter = value;
            }
        }

        internal static readonly List<IAppender> sharedAppenders = new List<IAppender>();
        private List<IAppender> appenders = null;

        public void AddAppender(IAppender appender)
        {
            if (this.appenders == null)
            {
                this.appenders = new List<IAppender>(sharedAppenders);
            }
            appenders.Add(appender);
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
            appenders.Remove(appender);
        }

        public void RemoveAppender<T>() where T : IAppender
        {
            if (this.appenders == null)
            {
                this.appenders = new List<IAppender>(sharedAppenders);
            }
            appenders.RemoveAll(a => a is T);
        }

        public void RemoveAllAppenders()
        {
            if (this.appenders == null)
            {
                this.appenders = new List<IAppender>();
            }
            appenders.Clear();
        }

        public void Debug(object message)
        {
            LogWithFilter(LogLevel.Debug, message);
        }

        public void Log(object message)
        {
            LogWithFilter(LogLevel.Log, message);
        }

        public void Warning(object message)
        {
            LogWithFilter(LogLevel.Warning, message);
        }

        public void Error(object message)
        {
            LogWithFilter(LogLevel.Error, message);
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
                string stackTrace = new StackTrace(3, true).ToString();
                foreach (var appender in appenders ?? sharedAppenders)
                {
                    try
                    {
                        IFormatter formatter = appender.formatter ?? defaultFormatter;
                        LogItem logItem = new LogItem()
                        {
                            logger = name,
                            logLevel = logLevel,
                            message = formatter.Format(
                                name,
                                logLevel,
                                message,
                                dateTime,
                                threadId,
                                stackTrace),
                            unityContext = null,
                        };
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
            Logger logger = LoggerFactory.GetLogger("UnityEngine.Debug");
            logger.RemoveAppender<UnityConsoleAppender>();

            switch (type)
            {
            case LogType.Warning:
                logger.Warning(condition);
                break;
            case LogType.Error:
                logger.Error(condition);
                break;
            default:
                logger.Log(condition);
                break;
            }
        }
    }
}
