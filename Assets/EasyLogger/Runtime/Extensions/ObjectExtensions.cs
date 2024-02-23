// -----------------------------------------------------------------------
// <copyright file="ObjectExtensions.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyLogger
{
    using UnityEngine;

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

        public static void Dump<T>(this T obj)
        {
            try
            {
                string json = JsonUtility.ToJson(obj);
                obj.Log(json);
            }
            catch
            {
                obj.Log(obj);
            }
        }

        public static void DumpAsError<T>(this T obj)
        {
            try
            {
                string json = JsonUtility.ToJson(obj);
                obj.LogError(json);
            }
            catch
            {
                obj.LogError(obj);
            }
        }
    }
}
