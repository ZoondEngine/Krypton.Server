using Krypton.Server.Core.IO;
using Microsoft.Extensions.Logging;
using System;

namespace Krypton.Server.Core.Database.Logging
{
    public class KeysContextLogger : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new KeysLogger();
        }

        public void Dispose() { }

        private class KeysLogger : ILogger
        {
            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                var print = IOMgr.Instance.GetPrint();
                var str = formatter(state, exception);

                switch (logLevel)
                {
                    case LogLevel.Error:
                    case LogLevel.Critical:
                        {
                            print.Error(str);
                            break;
                        }

                    case LogLevel.Warning:
                        {
                            print.Warning(str);
                            break;
                        }

                    default:
                        {
                            print.Trace(str);
                            break;
                        }
                }
            }
        }
    }
}
