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
            string folder = GetLogFolder();
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string path = Path.Combine(folder, $"{DateTime.Now:yyyyMMddHHmmssfff}.log");
            fileInfo = new FileInfo(path);
        }

        private string GetLogFolder()
        {
#if UNITY_EDITOR
            return Path.Combine(Application.dataPath, "..", "Logs");
#endif
            return Application.consoleLogPath;
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
            if (streamWriter == null)
            {
                if (fileInfo == null)
                {
                    return false;
                }
                else
                {
                    // FileUtils.Roller.FileRoll("");
                    streamWriter = fileInfo.CreateText();
                }
            }
            return true;
        }
    }
}
