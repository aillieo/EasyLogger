using UnityEngine;

namespace AillieoUtils.EasyLogger
{
    public class IMGUIAppender : IAppender
    {
        public IFormatter formatter { get; set; }

        private Alignment switcherAlignmentValue = Alignment.TopLeft;

        public Alignment switcherAlignment
        {
            get => switcherAlignment;
            set
            {
                if (switcherAlignmentValue != value)
                {
                    switcherAlignmentValue = value;
                    SetSwitcherWidgetPosition(switcherAlignmentValue);
                }
            }
        }

        public void OnReceiveLogItem(ref LogItem logItem)
        {
            EnsureInstance();
            instance.AppendLogItem(ref logItem);
        }

        public enum Alignment
        {
            TopLeft =       0b0000 | 0b0000,
            Top =           0b0001 | 0b0000,
            TopRight =      0b0010 | 0b0000,
            Left =          0b0000 | 0b0100,
            Center =        0b0001 | 0b0100,
            Right =         0b0010 | 0b0100,
            BottomLeft =    0b0000 | 0b1000,
            Bottom =        0b0001 | 0b1000,
            BottomRight =   0b0010 | 0b1000,
        }

        private void SetSwitcherWidgetPosition(Alignment alignment)
        {
            EnsureInstance();
            int horizontal = (int)alignment & 0b11;
            int vertical = ((int)alignment & 0b1100) >> 2;
            instance.SetSwitcherPosition(horizontal, vertical);
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
