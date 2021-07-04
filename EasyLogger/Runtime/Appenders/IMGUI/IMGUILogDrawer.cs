using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AillieoUtils.EasyLogger
{
    public class IMGUILogDrawer : MonoBehaviour
    {
        private bool drawLogItems = false;
        private readonly List<LogItem> records = new List<LogItem>();
        private IEnumerable<LogItem> recordsDisplay;
        private string textFilter = string.Empty;
        private LogLevel levelMask = LogLevel.Any;

        private GUIStyle guiStyleWindow;
        private GUIStyle guiStyleLog;
        private GUIStyle guiStyleButton;
        private GUIStyle guiStyleButtonSmall;
        private GUIStyle guiStyleTextField;
        private GUILayoutOption[] guiOptionsButtonSmall = new GUILayoutOption[] { GUILayout.Width(100) };
        private GUIContent infoicon;
        private GUIContent warnicon;
        private GUIContent erroricon;

        private Rect switcherRect = new Rect(0, 0, 100, 50);
        private Rect buttonRect = new Rect(25, 12.5f, 50, 25);
        private Vector2 scrollPosition = Vector2.zero;
        private bool dirty = false;

        internal void AppendLogItem(ref LogItem logItem)
        {
            records.Add(logItem);
            dirty = true;
        }

        private void Awake()
        {
            guiStyleButton = new GUIStyle("button");
            guiStyleButton.fontSize = 36;

            guiStyleButtonSmall = new GUIStyle("button");

            guiStyleTextField = new GUIStyle("textField");
            guiStyleTextField.fontSize = 36;

            guiStyleWindow = new GUIStyle("window");

            guiStyleLog = new GUIStyle("label");
            guiStyleLog.fontSize = 25;

            infoicon = new GUIContent(TextureAssets.Base64ToTexture(TextureAssets.infoicon));
            warnicon = new GUIContent(TextureAssets.Base64ToTexture(TextureAssets.warnicon));
            erroricon = new GUIContent(TextureAssets.Base64ToTexture(TextureAssets.erroricon));

            //for (int i = 0; i < 200; ++ i)
            //{
            //    records.Add(new LogItem { logLevel = LogLevel.Log, message = $"Log - <{i}>" });
            //    records.Add(new LogItem { logLevel = LogLevel.Debug, message = $"Debug - <{i}>" });
            //    records.Add(new LogItem { logLevel = LogLevel.Warning, message = $"Warning - <{i}>" });
            //    records.Add(new LogItem { logLevel = LogLevel.Error, message = $"Error - <{i}>" });
            //}
        }

        public void OnGUI()
        {
            if (drawLogItems)
            {
                DrawLogItems();
            }

            switcherRect = GUI.Window(0, switcherRect, DrawSwitcher, string.Empty, guiStyleWindow);
        }

        private void DrawLogItems()
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height), guiStyleWindow);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear", guiStyleButton))
            {
                Event.current.Use();
                if (records.Count > 0)
                {
                    dirty = true;
                    records.Clear();
                }
            }

            if (GUILayout.Button("Copy", guiStyleButton))
            {
                Event.current.Use();
                GUIUtility.systemCopyBuffer = string.Concat(recordsDisplay.Select(r => r.message));
            }

            GUILayout.Space(10);

            bool displayInfo = (levelMask & LogLevel.Log) > 0;
            bool displayWarning = (levelMask & LogLevel.Warning) > 0;
            bool displayError = (levelMask & LogLevel.Error) > 0;

            displayInfo = GUILayout.Toggle(displayInfo, infoicon, guiStyleButtonSmall, guiOptionsButtonSmall);
            displayWarning = GUILayout.Toggle(displayWarning, warnicon, guiStyleButtonSmall, guiOptionsButtonSmall);
            displayError = GUILayout.Toggle(displayError, erroricon, guiStyleButtonSmall, guiOptionsButtonSmall);

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

            string newTextFilter = GUILayout.TextField(textFilter, guiStyleTextField);
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
                GUILayout.Label(s.message, guiStyleLog);
            }

            GUILayout.EndScrollView();

            GUILayout.EndArea();
        }

        private void DrawSwitcher(int windowId)
        {
            if (GUI.Button(buttonRect, "EL", guiStyleButton))
            {
                drawLogItems = !drawLogItems;
            }
            GUI.DragWindow();
        }
    }
}
