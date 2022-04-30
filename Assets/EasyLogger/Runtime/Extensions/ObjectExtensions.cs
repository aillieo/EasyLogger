namespace AillieoUtils.EasyLogger
{
    public static class ObjectExtensions
    {
        public static void Debug<T>(this T obj, object message)
        {
            LoggerFactory.GetLogger<T>().Debug(message);
        }

        public static void Log<T>(this T obj, object message)
        {
            LoggerFactory.GetLogger<T>().Log(message);
        }

        public static void Warning<T>(this T obj, object message)
        {
            LoggerFactory.GetLogger<T>().Warning(message);
        }

        public static void Error<T>(this T obj, object message)
        {
            LoggerFactory.GetLogger<T>().Error(message);
        }
    }
}
