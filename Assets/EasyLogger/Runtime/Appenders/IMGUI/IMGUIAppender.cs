// -----------------------------------------------------------------------
// <copyright file="IMGUIAppender.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyLogger
{
    public class IMGUIAppender : IAppender
    {
        private Alignment switcherAlignmentValue = Alignment.TopLeft;

        public enum Alignment
        {
            TopLeft = 0b0000 | 0b0000,
            Top = 0b0001 | 0b0000,
            TopRight = 0b0010 | 0b0000,
            Left = 0b0000 | 0b0100,
            Center = 0b0001 | 0b0100,
            Right = 0b0010 | 0b0100,
            BottomLeft = 0b0000 | 0b1000,
            Bottom = 0b0001 | 0b1000,
            BottomRight = 0b0010 | 0b1000,
        }

        public IFormatter formatter { get; set; }

        public Alignment switcherAlignment
        {
            get => this.switcherAlignment;
            set
            {
                if (this.switcherAlignmentValue != value)
                {
                    this.switcherAlignmentValue = value;
                    this.SetSwitcherWidgetPosition(this.switcherAlignmentValue);
                }
            }
        }

        public void OnReceiveLogItem(ref LogItem logItem)
        {
            IMGUILogDrawer.Instance.AppendLogItem(ref logItem);
        }

        private void SetSwitcherWidgetPosition(Alignment alignment)
        {
            int horizontal = (int)alignment & 0b11;
            int vertical = ((int)alignment & 0b1100) >> 2;
            IMGUILogDrawer.Instance.SetSwitcherPosition(horizontal, vertical);
        }
    }
}
