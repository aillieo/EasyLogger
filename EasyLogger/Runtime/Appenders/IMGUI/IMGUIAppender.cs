using UnityEngine;

namespace AillieoUtils.EasyLogger
{
    public class IMGUIAppender : IAppender
    {
        public void OnReceiveLogItem(ref LogItem logItem)
        {
            EnsureInstance();
            instance.AppendLogItem(ref logItem);
        }

        private static IMGUILogDrawer instance;

        private static void EnsureInstance()
        {
            if (instance == null)
            {
                instance = new GameObject("IMGUILogDrawer").AddComponent<IMGUILogDrawer>();
                Object.DontDestroyOnLoad(instance.gameObject);
            }
        }
    }
}
