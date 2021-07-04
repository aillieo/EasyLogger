using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace AillieoUtils.EasyLogger
{
    public class LogFileWriter
    {
        FileInfo fileInfo = null;
        StreamWriter streamWriter = null;

        internal LogFileWriter()
        {
            string path = Path.Combine(Application.dataPath, $"{DateTime.Now:yyyyMMddHHmmssfff}.log");
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
            if (streamWriter == null)
            {
                if (fileInfo == null)
                {
                    return;
                }
                else
                {
                    streamWriter = fileInfo.CreateText();
                }
            }

            streamWriter.WriteLine(logItem.message);
        }
    }
}
