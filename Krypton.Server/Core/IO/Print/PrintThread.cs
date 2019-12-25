using System;
using System.Linq;

namespace Krypton.Server.Core.IO
{
    public class PrintThread
    {
        public void Error(string line, params string[] additional)
           => WriteLine(ConsoleColor.Red, Parse("-", ",", line, additional));

        public void Warning(string line, params string[] additional)
            => WriteLine(ConsoleColor.Yellow, Parse("!", ",", line, additional));

        public void Success(string line, params string[] additional)
            => WriteLine(ConsoleColor.Green, Parse("+", ",", line, additional));

        public void Trace(string line, params string[] additional)
            => WriteLine(ConsoleColor.Gray, Parse("&", ",", line, additional));

        private string Parse(string symbol, string delimeter, string line, params string[] additional)
        {
            string ready = $"[{symbol}]";
            if (additional.Length > 0)
            {
                ready += "[";

                foreach (var add in additional)
                {
                    ready += $"{add}";

                    if (add != additional.Last())
                        ready += $"{delimeter}";
                }

                ready += $"]";
            }

            ready += $": {line}";
            return ready;
        }
        private void WriteLine(ConsoleColor color, string readyLine)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(readyLine);
            Console.ResetColor();
        }
    }
}
