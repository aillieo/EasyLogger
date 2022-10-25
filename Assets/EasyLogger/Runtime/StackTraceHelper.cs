using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

namespace AillieoUtils.EasyLogger
{
    internal static class StackTraceHelper
    {
        private static Func<StackTrace, string> StackTraceUtility_ExtractFormattedStackTrace;

        internal static string Extract(int skipped)
        {
            if (StackTraceUtility_ExtractFormattedStackTrace == null)
            {
                try
                {
                    MethodInfo extractFormattedStackTrace = typeof(StackTraceUtility).GetMethod("ExtractFormattedStackTrace", BindingFlags.NonPublic | BindingFlags.Static);
                    StackTraceUtility_ExtractFormattedStackTrace = extractFormattedStackTrace.CreateDelegate(typeof(Func<StackTrace, string>)) as Func<StackTrace, string>;
                }
                catch
                {
                    StackTraceUtility_ExtractFormattedStackTrace = st => st.ToString();
                }
            }

            StackTrace stackTrace = new StackTrace(skipped + 1, true);
            return StackTraceUtility_ExtractFormattedStackTrace(stackTrace);
        }
    }
}
