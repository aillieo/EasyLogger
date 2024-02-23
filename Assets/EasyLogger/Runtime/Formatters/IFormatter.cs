// -----------------------------------------------------------------------
// <copyright file="IFormatter.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyLogger
{
    using System;

    public interface IFormatter
    {
        string Format(string logger, LogLevel logLevel, object message, DateTime time, int thread, string stackTrace);
    }
}
