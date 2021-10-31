namespace AillieoUtils.EasyLogger
{
    public class UnityConsoleAppender : IAppender
    {
        public IFormatter formatter { get; set; }

        private int internalCall = 0;

        public void OnReceiveLogItem(ref LogItem logItem)
        {
            if (internalCall > 0)
            {
                return;
            }

            internalCall++;

            try
            {
                UnityConsoleLog(logItem.logLevel, logItem.message);
            }
            finally
            {
                internalCall--;
            }
        }

        private void UnityConsoleLog(LogLevel logLevel, string message)
        {
            message = ConsoleHyperlinkWrapper.Wrap(message);

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
