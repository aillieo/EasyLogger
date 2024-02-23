// -----------------------------------------------------------------------
// <copyright file="IMGUILogDrawer.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyLogger
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    internal class IMGUILogDrawer : SingletonMonoBehaviour<IMGUILogDrawer>
    {
        private readonly List<LogItem> records = new List<LogItem>();
        private bool drawLogItems = false;
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

        public void OnGUI()
        {
            if (this.drawLogItems)
            {
                this.DrawLogItems();
            }

            this.switcherRect = GUI.Window(0, this.switcherRect, this.DrawSwitcher, string.Empty, GUIStyles.guiStyleWindow);
        }

        internal void AppendLogItem(ref LogItem logItem)
        {
            this.records.Add(logItem);
            this.dirty = true;
        }

        internal void SetSwitcherPosition(int horizontal = 0, int vertical = 0)
        {
            float xMin = (-this.switcherRect.width / 2) + this.switcherBorder;
            float yMin = (-this.switcherRect.height / 2) + this.switcherBorder;
            float xMax = Screen.width + (this.switcherRect.width / 2) - this.switcherBorder;
            float yMax = Screen.height + (this.switcherRect.height / 2) - this.switcherBorder;
            this.switcherRect.center = new Vector2(Mathf.Lerp(xMin, xMax, horizontal / 2f), Mathf.Lerp(yMin, yMax, vertical / 2f));
        }

        protected override void Awake()
        {
            base.Awake();

            this.logoicon = new GUIContent(TextureAssets.Base64ToTexture(TextureAssets.logo));
            this.infoicon = new GUIContent(TextureAssets.Base64ToTexture(TextureAssets.infoicon));
            this.warnicon = new GUIContent(TextureAssets.Base64ToTexture(TextureAssets.warnicon));
            this.erroricon = new GUIContent(TextureAssets.Base64ToTexture(TextureAssets.erroricon));

            this.SetSwitcherPosition();
        }

        private void DrawLogItems()
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height), GUIStyles.guiStyleWindow);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear", GUIStyles.guiStyleButton))
            {
                if (this.records.Count > 0)
                {
                    this.dirty = true;
                    this.records.Clear();
                }
            }

            if (GUILayout.Button("Copy", GUIStyles.guiStyleButton))
            {
                GUIUtility.systemCopyBuffer = string.Concat(this.recordsDisplay.Select(r => r.message));
            }

            GUILayout.Space(10);

            bool displayInfo = (this.levelMask & LogLevel.Log) > 0;
            bool displayWarning = (this.levelMask & LogLevel.Warning) > 0;
            bool displayError = (this.levelMask & LogLevel.Error) > 0;

            displayInfo = GUILayout.Toggle(displayInfo, this.infoicon, GUIStyles.guiStyleButtonSmall, this.guiOptionsButtonSmall);
            displayWarning = GUILayout.Toggle(displayWarning, this.warnicon, GUIStyles.guiStyleButtonSmall, this.guiOptionsButtonSmall);
            displayError = GUILayout.Toggle(displayError, this.erroricon, GUIStyles.guiStyleButtonSmall, this.guiOptionsButtonSmall);

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

            if (this.levelMask != newLevelMask)
            {
                this.levelMask = newLevelMask;
                this.dirty = true;
            }

            GUILayout.EndHorizontal();

            string newTextFilter = GUILayout.TextField(this.textFilter, GUIStyles.guiStyleTextField);
            if (this.textFilter != newTextFilter)
            {
                this.textFilter = newTextFilter;
                this.dirty = true;
            }

            this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition);

            if (this.dirty)
            {
                this.dirty = false;
                this.recordsDisplay = this.records.Where(l => (l.logLevel & this.levelMask) > 0);
                if (!string.IsNullOrEmpty(this.textFilter))
                {
                    this.recordsDisplay = this.records.Where(s => s.message.Contains(this.textFilter));
                }
            }

            foreach (var s in this.recordsDisplay)
            {
                GUILayout.Label(s.message, GUIStyles.guiStyleLog);
            }

            GUILayout.EndScrollView();

            GUILayout.EndArea();
        }

        private void DrawSwitcher(int windowId)
        {
            Rect excludeBorder = new Rect(
                Vector2.zero + (this.switcherBorder * Vector2.one),
                this.switcherRect.size - (2 * this.switcherBorder * Vector2.one));
            if (GUI.Button(excludeBorder, this.logoicon, GUIStyles.guiStyleButton))
            {
                this.drawLogItems = !this.drawLogItems;
            }

            GUI.DragWindow();
        }

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
    }
}
