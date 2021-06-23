using System;
using System.Collections.Generic;

namespace AillieoUtils.EasyLogger
{
    public class Logger
    {
        internal Logger(string name)
        {

        }

        private List<IAppender> appenders = new List<IAppender>();

        public LogLevel filter { get; set; } = LogLevel.Any;

        public void AddAppender(IAppender appender)
        {
            appenders.Add(appender);
        }

        public void RemoveAllAppenders()
        {
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
                LogItem logItem = new LogItem() { logLevel = logLevel, message = Convert.ToString(message) };
                Dispatch(logItem);
            }
        }

        private void Dispatch(LogItem logItem)
        {
            foreach (var a in appenders)
            {
                a.OnReceiveLogItem(ref logItem);
            }
        }
    }

}
