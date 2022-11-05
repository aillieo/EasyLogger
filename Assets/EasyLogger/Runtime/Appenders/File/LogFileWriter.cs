using System;
using System.Collections.Generic;
using System.IO;

namespace AillieoUtils.EasyLogger
{
    public class LogFileWriter
    {
        private FileInfo fileInfo = null;
        private StreamWriter streamWriter = null;

        internal LogFileWriter(string path)
        {
            fileInfo = new FileInfo(path);
        }

        internal void Dispose()
        {
            if (fileInfo == null)
            {
                return;
            }

            if (streamWriter != null)
            {
                streamWriter.Flush();
                streamWriter.Close();
                streamWriter.Dispose();
                streamWriter = null;
            }

            fileInfo = null;
        }

        internal void AppendLogItem(ref LogItem logItem)
        {
            if (!EnsureWriter())
            {
                return;
            }

            streamWriter.WriteLine(logItem.message);
        }

        private bool EnsureWriter()
        {
            if (fileInfo == null)
            {
                return false;
            }

            if (streamWriter == null)
            {
                streamWriter = fileInfo.CreateText();
                streamWriter.AutoFlush = true;
            }

            return true;
        }
    }
}
