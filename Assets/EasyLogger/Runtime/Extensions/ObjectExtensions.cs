namespace AillieoUtils.EasyLogger
{
    public static class ObjectExtensions
    {
        public static Logger GetLogger<T>(this T obj)
        {
            return LoggerFactory.GetLogger<T>();
        }

        public static void LogDebug(this object obj, object message)
        {
            obj.GetLogger().Debug(message);
        }

        public static void Log(this object obj, object message)
        {
            obj.GetLogger().Log(message);
        }

        public static void LogWarning(this object obj, object message)
        {
            obj.GetLogger().Warning(message);
        }

        public static void LogError(this object obj, object message)
        {
            obj.GetLogger().Error(message);
        }
    }
}
