using System;
using System.Collections.Generic;

namespace AillieoUtils.EasyLogger
{
    public static class LoggerFactory
    {
        private static readonly Dictionary<string, Logger> cachedInstances = new Dictionary<string, Logger>();

        public static Logger GetLogger(string name)
        {
            if (!cachedInstances.TryGetValue(name, out Logger instance))
            {
                instance = new Logger(name);
                cachedInstances.Add(name, instance);
            }

            return instance;
        }

        public static Logger GetLogger<T>()
        {
            return GetLogger(typeof(T).Name);
        }
    }
}
