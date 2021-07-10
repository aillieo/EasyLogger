using System;
using System.Collections.Generic;
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

        private LogLevel? instanceFilter = null;
        public static LogLevel sharedFilter = LogLevel.Any;

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

        private static readonly List<IAppender> sharedAppenders = new List<IAppender>();
        private List<IAppender> appenders = null;

        public void AddAppender(IAppender appender)
        {
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

        public void RemoveAllAppenders()
        {
            if (this.appenders == null)
            {
                this.appenders = new List<IAppender>(sharedAppenders);
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

        private void LogWithFilter(LogLevel logLevel, object message)
        {
            if ((this.filter & logLevel) > 0)
            {
                foreach (var appender in appenders ?? sharedAppenders)
                {
                    try
                    {
                        IFormatter formatter = appender.formatter ?? defaultFormatter;
                        LogItem logItem = new LogItem() { logLevel = logLevel, message = formatter.Format(message) };
                        appender.OnReceiveLogItem(ref logItem);
                    }
                    catch
                    {
                    }
                }
            }
        }

        private static void OnUnityLogEvent(string condition, string stackTrace, LogType type)
        {
            Logger logger = LoggerFactory.GetLogger("Unity");
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