using System;

namespace AillieoUtils.EasyLogger
{
    [Serializable]
    public class EasyLoggerConfig
    {
        public bool receiveUnityLogEvents;
        public LogLevel filter;
        public bool imGuiAppender;
        public bool fileAppender;
        public bool unityConsoleAppender;
    }
}
