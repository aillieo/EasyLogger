using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils.EasyLogger
{
    public class IMGUILogDrawer : MonoBehaviour
    {
        private readonly List<string> records = new List<string>();

        internal void AppendLogItem(ref LogItem logItem)
        {
            records.Add(logItem.message);
        }

        private void OnGUI()
        {
            foreach (var s in records)
            {
                GUILayout.Label(s);
            }
        }
    }
}
