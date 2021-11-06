using System;

namespace AillieoUtils.EasyLogger
{
    public interface IFormatter
    {
        string Format(string logger, LogLevel logLevel, object message, DateTime time, int thread, string stackTrace);
    }
}
