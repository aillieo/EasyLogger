// -----------------------------------------------------------------------
// <copyright file="DefaultFormatter.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyLogger
{
    using System;

    public class DefaultFormatter : IFormatter
    {
        public string Format(string logger, LogLevel logLevel, object message, DateTime time, int thread, string stackTrace)
        {
            return $"[{time:MM/dd/yyyy HH:mm:ss:fff}]|{this.LogLevelToChar(ref logLevel)}|{logger}|{thread}|{message}{stackTrace}";
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
