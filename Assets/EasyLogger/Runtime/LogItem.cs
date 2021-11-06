using System;

namespace AillieoUtils.EasyLogger
{
    public struct LogItem
    {
        public string logger;
        public LogLevel logLevel;
        public string message;
        public UnityEngine.Object unityContext;
    }
}
