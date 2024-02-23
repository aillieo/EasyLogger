// -----------------------------------------------------------------------
// <copyright file="LogItem.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyLogger
{
    public readonly struct LogItem
    {
        public readonly string logger;
        public readonly LogLevel logLevel;
        public readonly string message;
        public readonly UnityEngine.Object unityContext;

        public LogItem(string logger, LogLevel logLevel, string message, UnityEngine.Object unityContext)
        {
            this.logger = logger;
            this.logLevel = logLevel;
            this.message = message;
            this.unityContext = unityContext;
        }
    }
}
