// -----------------------------------------------------------------------
// <copyright file="UnityConsoleAppender.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyLogger
{
    public partial class UnityConsoleAppender : IAppender
    {
        private int internalCall = 0;

        public IFormatter formatter { get; set; } = new UnityConsoleLogFormatter();

        public void OnReceiveLogItem(ref LogItem logItem)
        {
            if (this.internalCall > 0)
            {
                return;
            }

            this.internalCall++;

            try
            {
                this.UnityConsoleLog(logItem.logLevel, logItem.message);
            }
            finally
            {
                this.internalCall--;
            }
        }

        private void UnityConsoleLog(LogLevel logLevel, string message)
        {
            switch (logLevel)
            {
                case LogLevel.Warning:
                    UnityEngine.Debug.LogWarning(message);
                    break;
                case LogLevel.Error:
                    UnityEngine.Debug.LogError(message);
                    break;
                default:
                    UnityEngine.Debug.Log(message);
                    break;
            }
        }
    }
}
