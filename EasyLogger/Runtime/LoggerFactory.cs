using System;

namespace AillieoUtils.EasyLogger
{
    public static class LoggerFactory
    {
        public static Logger CreateLogger(string name)
        {
            return new Logger(name);
        }

        public static Logger CreateLogger<T>()
        {
            return CreateLogger(typeof(T).Name);
        }
    }
}
