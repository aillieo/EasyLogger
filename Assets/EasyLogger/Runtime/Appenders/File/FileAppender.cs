// -----------------------------------------------------------------------
// <copyright file="FileAppender.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyLogger
{
    using System;
    using System.IO;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    public class FileAppender : IAppender
    {
        private object lockObj = new object();

        private LogFileWriter writer;

        internal FileAppender(int maxFileCountKept, int maxDaysKept)
        {
            try
            {
                string folder = GetLogFolder();

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                var rollConfig = FileUtils.Roller.RollConfig.Default;
                rollConfig.maxFileCount = maxFileCountKept;
                FileUtils.Roller.FileRoll(folder, rollConfig);

                string path = Path.Combine(folder, $"{DateTime.Now:yyyyMMddHHmmssfff}.log");

                this.writer = new LogFileWriter(path);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
            finally
            {
                ApplicationEvents.onApplicationQuit += this.OnApplicationQuit;
            }
        }

        public IFormatter formatter { get; set; }

        public void OnReceiveLogItem(ref LogItem logItem)
        {
            if (this.writer == null)
            {
                return;
            }

            lock (this.lockObj)
            {
                this.writer.AppendLogItem(ref logItem);
            }
        }

        private static string GetLogFolder()
        {
            return FileUtils.GetPersistentPath("Logs");
        }

#if UNITY_EDITOR
        [MenuItem("AillieoUtils/EasyLogger/LocateLogFolder", false)]
        private static void LocateLogFolder()
        {
            EditorUtility.RevealInFinder(GetLogFolder());
        }
#endif

        private void OnApplicationQuit()
        {
            if (this.writer != null)
            {
                this.writer.Dispose();
                this.writer = null;
            }
        }
    }
}
