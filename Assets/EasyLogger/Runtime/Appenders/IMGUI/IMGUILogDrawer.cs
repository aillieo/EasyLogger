using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AillieoUtils.EasyLogger
{
    public class IMGUILogDrawer : SingletonMonoBehaviour<IMGUILogDrawer>
    {
        private static class GUIStyles
        {
            public static GUIStyle guiStyleWindow;
            public static GUIStyle guiStyleLog;
            public static GUIStyle guiStyleButton;
            public static GUIStyle guiStyleButtonSmall;
            public static GUIStyle guiStyleTextField;

            static GUIStyles()
            {
                guiStyleButton = new GUIStyle("button");
                guiStyleButton.fontSize = 36;

                guiStyleButtonSmall = new GUIStyle("button");

                guiStyleTextField = new GUIStyle("textField");
                guiStyleTextField.fontSize = 36;

                guiStyleWindow = new GUIStyle("window");

                guiStyleLog = new GUIStyle("label");
                guiStyleLog.fontSize = 25;
            }
        }

        private bool drawLogItems = false;
        private readonly List<LogItem> records = new List<LogItem>();
        private IEnumerable<LogItem> recordsDisplay;
        private string textFilter = string.Empty;
        private LogLevel levelMask = LogLevel.Any;

        private GUILayoutOption[] guiOptionsButtonSmall = new GUILayoutOption[] { GUILayout.Width(100) };
        private GUIContent logoicon;
        private GUIContent infoicon;
        private GUIContent warnicon;
        private GUIContent erroricon;

        private Rect switcherRect = new Rect(0, 0, 100, 100);
        private int switcherBorder = 20;
        private Vector2 scrollPosition = Vector2.zero;
        private bool dirty = false;

        internal void AppendLogItem(ref LogItem logItem)
        {
            records.Add(logItem);
            dirty = true;
        }

        protected override void Awake()
        {
            base.Awake();

            logoicon = new GUIContent(TextureAssets.Base64ToTexture(TextureAssets.logo));
            infoicon = new GUIContent(TextureAssets.Base64ToTexture(TextureAssets.infoicon));
            warnicon = new GUIContent(TextureAssets.Base64ToTexture(TextureAssets.warnicon));
            erroricon = new GUIContent(TextureAssets.Base64ToTexture(TextureAssets.erroricon));

            SetSwitcherPosition();
        }

        public void OnGUI()
        {
            if (drawLogItems)
            {
                DrawLogItems();
            }

            switcherRect = GUI.Window(0, switcherRect, DrawSwitcher, string.Empty, GUIStyles.guiStyleWindow);
        }

        internal void SetSwitcherPosition(int horizontal = 0, int vertical = 0)
        {
            float xMin = -switcherRect.width / 2 + switcherBorder;
            float yMin = -switcherRect.height / 2 + switcherBorder;
            float xMax = Screen.width + switcherRect.width / 2 - switcherBorder;
            float yMax = Screen.height + switcherRect.height / 2 - switcherBorder;
            switcherRect.center = new Vector2(Mathf.Lerp(xMin, xMax, horizontal / 2f), Mathf.Lerp(yMin, yMax, vertical / 2f));
        }

        private void DrawLogItems()
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height), GUIStyles.guiStyleWindow);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear", GUIStyles.guiStyleButton))
            {
                if (records.Count > 0)
                {
                    dirty = true;
                    records.Clear();
                }
            }

            if (GUILayout.Button("Copy", GUIStyles.guiStyleButton))
            {
                GUIUtility.systemCopyBuffer = string.Concat(recordsDisplay.Select(r => r.message));
            }

            GUILayout.Space(10);

            bool displayInfo = (levelMask & LogLevel.Log) > 0;
            bool displayWarning = (levelMask & LogLevel.Warning) > 0;
            bool displayError = (levelMask & LogLevel.Error) > 0;

            displayInfo = GUILayout.Toggle(displayInfo, infoicon, GUIStyles.guiStyleButtonSmall, guiOptionsButtonSmall);
            displayWarning = GUILayout.Toggle(displayWarning, warnicon, GUIStyles.guiStyleButtonSmall, guiOptionsButtonSmall);
            displayError = GUILayout.Toggle(displayError, erroricon, GUIStyles.guiStyleButtonSmall, guiOptionsButtonSmall);

            LogLevel newLevelMask = LogLevel.None;
            if (displayInfo)
            {
                newLevelMask |= LogLevel.Log;
                newLevelMask |= LogLevel.Debug;
            }

            if (displayWarning)
            {
                newLevelMask |= LogLevel.Warning;
            }

            if (displayError)
            {
                newLevelMask |= LogLevel.Error;
            }

            if (levelMask != newLevelMask)
            {
                levelMask = newLevelMask;
                dirty = true;
            }

            GUILayout.EndHorizontal();

            string newTextFilter = GUILayout.TextField(textFilter, GUIStyles.guiStyleTextField);
            if (textFilter != newTextFilter)
            {
                textFilter = newTextFilter;
                dirty = true;
            }

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            if (dirty)
            {
                dirty = false;
                recordsDisplay = records.Where(l => (l.logLevel & levelMask) > 0);
                if (!string.IsNullOrEmpty(textFilter))
                {
                    recordsDisplay = records.Where(s => s.message.Contains(textFilter));
                }
            }

            foreach (var s in recordsDisplay)
            {
                GUILayout.Label(s.message, GUIStyles.guiStyleLog);
            }

            GUILayout.EndScrollView();

            GUILayout.EndArea();
        }

        private void DrawSwitcher(int windowId)
        {
            Rect excludeBorder = new Rect(
                Vector2.zero + switcherBorder * Vector2.one,
                switcherRect.size - 2 * switcherBorder * Vector2.one);
            if (GUI.Button(excludeBorder, logoicon, GUIStyles.guiStyleButton))
            {
                drawLogItems = !drawLogItems;
            }

            GUI.DragWindow();
        }
    }
}
