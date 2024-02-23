// -----------------------------------------------------------------------
// <copyright file="UnityConsoleLogFormatter.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.EasyLogger
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    internal class UnityConsoleLogFormatter : IFormatter
    {
        private static readonly Regex regex = new Regex(@"(.+\[[\d\w]+\] in )([\S]+.cs)(:(\d+))");
        private static readonly string rep = @"$1<a href=""$2"" line=""$4"">$2$3</a>";

        // ConsoleWindow.StacktraceWithHyperlinks(this.m_ActiveText);
        private static Func<string, string> ConsoleWindow_StacktraceWithHyperlinks;

        public string Format(string logger, LogLevel logLevel, object message, DateTime time, int thread, string stackTrace)
        {
            if (string.IsNullOrWhiteSpace(stackTrace))
            {
                return ProcessStacktrace(Convert.ToString(message, CultureInfo.InvariantCulture));
            }
            else
            {
                return $"{ProcessStacktrace(Convert.ToString(message, CultureInfo.InvariantCulture))}{ProcessStacktrace(stackTrace)}";
            }
        }

        internal static string ProcessStacktrace(string rawString)
        {
            if (ConsoleWindow_StacktraceWithHyperlinks == null)
            {
                try
                {
                    Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "UnityEditor.CoreModule");
                    Type consoleWindowType = assembly.GetType("UnityEditor.ConsoleWindow");
                    MethodInfo stacktraceWithHyperlinks = consoleWindowType.GetMethod("StacktraceWithHyperlinks", BindingFlags.Static | BindingFlags.NonPublic);
                    var parameters = stacktraceWithHyperlinks.GetParameters();
                    if (parameters.Length == 1)
                    {
                        ConsoleWindow_StacktraceWithHyperlinks = stacktraceWithHyperlinks.CreateDelegate(typeof(Func<string, string>)) as Func<string, string>;
                    }
                    else
                    {
                        Func<string, int, string> f = stacktraceWithHyperlinks.CreateDelegate(typeof(Func<string, int, string>)) as Func<string, int, string>;
                        ConsoleWindow_StacktraceWithHyperlinks = text => f(text, 0);
                    }
                }
                catch
                {
                    ConsoleWindow_StacktraceWithHyperlinks = StacktraceWithHyperlinks;
                }
            }

            return ConsoleWindow_StacktraceWithHyperlinks(rawString);
        }

        private static string StacktraceWithHyperlinks(string rawString)
        {
            string[] lines = rawString.Split('\n');
            if (lines.Length == 0)
            {
                return rawString;
            }

            StringBuilder stringBuilder = new StringBuilder();
            foreach (string line in lines)
            {
                stringBuilder.AppendLine(regex.Replace(line, rep));
            }

            return stringBuilder.ToString();
        }
    }
}
