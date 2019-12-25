using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading;

namespace Krypton.Support.CodeAnalyzer
{
    public class Analyze : IDisposable
    {
        private Stopwatch _sw;
        private string _label;

        public static Analyze Watch(string lable)
        {
            return new Analyze(lable);
        }

        Analyze(string label)
        {
            _label = label;
            _sw = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _sw.Stop();
            Console.WriteLine($"[ANALYZER] >> {_label} : {_sw.Elapsed}");
        }
    }
}
