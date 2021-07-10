using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AillieoUtils.EasyLogger
{
    public class FileAppender : IAppender
    {
        public IFormatter formatter { get; set; }

        private static LogFileWriter writer;

        public void OnReceiveLogItem(ref LogItem logItem)
        {
            if (writer == null)
            {
                return;
            }

            writer.AppendLogItem(ref logItem);
        }

        private static void OnApplicationQuit()
        {
            if (writer != null)
            {
                writer.Dispose();
                writer = null;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnSessionLaunch()
        {
            if (writer == null)
            { writer = new LogFileWriter(); }
            Application.quitting += OnApplicationQuit;
        }

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void OnEditorLaunch()
        {
            EditorApplication.playModeStateChanged += (state) =>
            {
                if (state == UnityEditor.PlayModeStateChange.ExitingEditMode)
                {
                    OnEditorApplicationQuit();
                }
            };
        }

        private static void OnEditorApplicationQuit()
        {
            if (writer != null)
            {
                writer.Dispose();
                writer = null;
            }
        }
#endif
    }
}
