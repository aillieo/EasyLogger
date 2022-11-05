using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace AillieoUtils.EasyLogger
{
    public class FileAppender : IAppender
    {
        public IFormatter formatter { get; set; }

        private LogFileWriter writer;

        public void OnReceiveLogItem(ref LogItem logItem)
        {
            if (writer == null)
            {
                return;
            }

            writer.AppendLogItem(ref logItem);
        }

        private void OnApplicationQuit()
        {
            if (writer != null)
            {
                writer.Dispose();
                writer = null;
            }
        }

        private static string GetLogFolder()
        {
#if UNITY_EDITOR
            return Path.Combine(Application.dataPath, "..", "Logs");
#else
            return Path.Combine(Application.persistentDataPath, "Logs");
#endif
        }

#if UNITY_EDITOR
        [MenuItem("AillieoUtils/EasyLogger/LocateLogFolder", false)]
        private static void LocateLogFolder()
        {
            EditorUtility.RevealInFinder(GetLogFolder());
        }
#endif

        public FileAppender(int maxFileCountKept, int maxDaysKept)
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

                writer = new LogFileWriter(path);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
            finally
            {
                ApplicationEvents.onApplicationQuit += OnApplicationQuit;
            }
        }
    }
}
