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

        public enum Alignment
        {
            TopLeft =       0x0000 | 0x0000,
            Top =           0x0001 | 0x0000,
            TopRight =      0x0010 | 0x0000,
            Left =          0x0000 | 0x0100,
            Center =        0x0001 | 0x0100,
            Right =         0x0010 | 0x0100,
            BottomLeft =    0x0000 | 0x1000,
            Bottom =        0x0001 | 0x1000,
            BottomRight =   0x0010 | 0x1000,
        }

        public Alignment switcherWidgetPosition;

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
