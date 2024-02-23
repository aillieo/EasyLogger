// -----------------------------------------------------------------------
// <copyright file="IAppender.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyLogger
{
    public interface IAppender
    {
        IFormatter formatter { get; set; }

        void OnReceiveLogItem(ref LogItem logItem);
    }
}
