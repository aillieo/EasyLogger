using System.Text;
using System.Text.RegularExpressions;

namespace AillieoUtils.EasyLogger
{
    internal static class ConsoleHyperlinkWrapper
    {
        private static readonly Regex regex = new Regex(@"(at \S+ \([\w\d\s_.]*\) \[[\d\w]+\] in )([\S]+.cs)(:(\d+))");
        private static readonly string rep = @"$1<a href=""$2"" line=""$4"">$2$3</a>";

        public static string Wrap(string rawString)
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
