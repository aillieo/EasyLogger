// -----------------------------------------------------------------------
// <copyright file="LogFileWriter.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyLogger
{
    using System.IO;

    internal class LogFileWriter
    {
        private FileInfo fileInfo = null;
        private StreamWriter streamWriter = null;

        internal LogFileWriter(string path)
        {
            this.fileInfo = new FileInfo(path);
        }

        internal void Dispose()
        {
            if (this.fileInfo == null)
            {
                return;
            }

            if (this.streamWriter != null)
            {
                this.streamWriter.Flush();
                this.streamWriter.Close();
                this.streamWriter.Dispose();
                this.streamWriter = null;
            }

            this.fileInfo = null;
        }

        internal void AppendLogItem(ref LogItem logItem)
        {
            if (!this.EnsureWriter())
            {
                return;
            }

            this.streamWriter.WriteLine(logItem.message);
        }

        private bool EnsureWriter()
        {
            if (this.fileInfo == null)
            {
                return false;
            }

            if (this.streamWriter == null)
            {
                this.streamWriter = this.fileInfo.CreateText();
                this.streamWriter.AutoFlush = true;
            }

            return true;
        }
    }
}
