using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace AillieoUtils.EasyLogger
{
    internal static class UnityConsoleHyperlinkWrapper
    {
        // ConsoleWindow.StacktraceWithHyperlinks(this.m_ActiveText);

        private static Func<string, string> ConsoleWindow_StacktraceWithHyperlinks;

        private static readonly Regex regex = new Regex(@"(.+\[[\d\w]+\] in )([\S]+.cs)(:(\d+))");
        private static readonly string rep = @"$1<a href=""$2"" line=""$4"">$2$3</a>";

        public static string Wrap(string rawString)
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
