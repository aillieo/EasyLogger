using System;
namespace AillieoUtils.EasyLogger
{
    [Flags]
    public enum LogLevel
    {
        None = 0,
        Debug = 1,
        Log = 2,
        Warning = 4,
        Error = 8,
        NonDebug = 14,
        Any = 15,
    }
}
