using System;

namespace AillieoUtils.EasyLogger
{
    public class DefaultFormatter : IFormatter
    {
        public string Format(string logger, LogLevel logLevel, object message, DateTime time, int thread, string stackTrace)
        {
            if (string.IsNullOrWhiteSpace(stackTrace))
            {
                return $"[{time}][{LogLevelToChar(ref logLevel)}]{logger} {thread}{Environment.NewLine}{message}";
            }
            else
            {
                return $"[{time}][{LogLevelToChar(ref logLevel)}]{logger} {thread}{Environment.NewLine}{message}{Environment.NewLine}{stackTrace}";
            }
        }

        private char LogLevelToChar(ref LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    return 'D';
                case LogLevel.Error:
                    return 'E';
                case LogLevel.Log:
                    return 'I';
                case LogLevel.Warning:
                    return 'W';
                default:
                    return ' ';
            }
        }
    }
}
