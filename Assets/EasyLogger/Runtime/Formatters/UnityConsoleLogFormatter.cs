using System;
using UnityEngine;

namespace AillieoUtils.EasyLogger
{
    public class UnityConsoleLogFormatter : IFormatter
    {
        public string Format(string logger, LogLevel logLevel, object message, DateTime time, int thread, string stackTrace)
        {
            if (string.IsNullOrWhiteSpace(stackTrace))
            {
                return UnityConsoleHyperlinkWrapper.Wrap(Convert.ToString(message));
            }
            else
            {
                return $"{UnityConsoleHyperlinkWrapper.Wrap(Convert.ToString(message))}{UnityConsoleHyperlinkWrapper.Wrap(Convert.ToString(stackTrace))}";
            }
        }
    }
}
