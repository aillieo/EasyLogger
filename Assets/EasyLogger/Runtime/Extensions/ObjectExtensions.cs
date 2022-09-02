namespace AillieoUtils.EasyLogger
{
    public static class ObjectExtensions
    {
        public static Logger GetLogger<T>(this T obj)
        {
            return LoggerFactory.GetLogger<T>();
        }

        public static void LogDebug<T>(this T obj, object message)
        {
            obj.GetLogger().Debug(message);
        }

        public static void Log<T>(this T obj, object message)
        {
            obj.GetLogger().Log(message);
        }

        public static void LogWarning<T>(this T obj, object message)
        {
            obj.GetLogger().Warning(message);
        }

        public static void LogError<T>(this T obj, object message)
        {
            obj.GetLogger().Error(message);
        }
    }
}
