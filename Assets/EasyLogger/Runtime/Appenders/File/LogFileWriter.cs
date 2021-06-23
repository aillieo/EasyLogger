using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace AillieoUtils.EasyLogger
{
    public class LogFileWriter
    {
        StreamWriter streamWriter = null;

        internal LogFileWriter()
        {
            string path = Path.Combine(Application.dataPath, $"{DateTime.Now:yyyyMMddHHmmssfff}.log");
            UnityEngine.Debug.LogError("path = " + path);
            streamWriter = File.CreateText(path);
        }

        internal void Dispose()
        {
            if (streamWriter == null)
            {
                return;
            }

            streamWriter.Flush();
            streamWriter.Close();
            streamWriter.Dispose();
            streamWriter = null;
        }

        internal void AppendLogItem(ref LogItem logItem)
        {
            if (streamWriter == null)
            {
                return;
            }

            streamWriter.WriteLine(logItem.message);
        }
    }
}
