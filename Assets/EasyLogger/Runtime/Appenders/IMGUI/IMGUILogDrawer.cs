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
        private string filter = string.Empty;

        private GUIStyle guiStyleWindow;
        private GUIStyle guiStyleLog;
        private GUIStyle guiStyleButton;
        private GUIStyle guiStyleButtonSmall;
        private GUIStyle guiStyleTextField;
        private Rect switcherRect = new Rect(0, 0, 100, 50);
        private Rect buttonRect = new Rect(25, 12.5f, 50, 25);
        private Vector2 scrollPosition = Vector2.zero;

        internal void AppendLogItem(ref LogItem logItem)
        {
            records.Add(logItem);
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

            for (int i = 0; i < 200; ++ i)
            {
                records.Add(new LogItem { logLevel = LogLevel.None, message = $"log - <{i}>" });
            }
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
            GUILayout.BeginArea(new Rect(0, 0, 1600, Screen.height), new GUIStyle("window"));

            GUILayout.BeginHorizontal();
            GUILayout.Button("Clear", guiStyleButton);
            GUILayout.Button("Copy", guiStyleButton);
            GUILayout.Space(10);
            GUILayout.Button(EditorGUIUtility.IconContent("console.infoicon"));
            GUILayout.Button(EditorGUIUtility.IconContent("console.warnicon"));
            GUILayout.Button(EditorGUIUtility.IconContent("console.erroricon"));
            GUILayout.EndHorizontal();

            filter = GUILayout.TextField(filter, guiStyleTextField);

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            IEnumerable<LogItem> toDraw = records;
            if (!string.IsNullOrEmpty(filter))
            {
                toDraw = records.Where(s => s.message.Contains(filter));
            }
            foreach (var s in toDraw)
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
