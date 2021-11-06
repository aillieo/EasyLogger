using System;

namespace AillieoUtils.EasyLogger
{
    public class UnityConsoleLogFormatter : IFormatter
    {
        public string Format(string logger, LogLevel logLevel, object message, DateTime time, int thread, string stackTrace)
        {
            if (string.IsNullOrWhiteSpace(stackTrace))
            {
                return ConsoleHyperlinkWrapper.Wrap(Convert.ToString(message));
            }
            else
            {
                return $"{ConsoleHyperlinkWrapper.Wrap(Convert.ToString(message))}{ConsoleHyperlinkWrapper.Wrap(Convert.ToString(stackTrace))}";
            }
        }
    }
}
