using Krypton.Server.Core.IO.Commands.Detail;
using Krypton.Server.Core.IO.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Principal;

namespace Krypton.Server.Core.IO
{
    public class CommandThread
    {
        private List<ICommandElement> RegisteredCommands;
        bool IsDisabled;
        private string User = WindowsIdentity.GetCurrent().Name;
        private string Ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.ToString();

        public CommandThread()
        {
            RegisteredCommands = Load();
            IsDisabled = false;
        }

        public CommandDoResult DoProcess()
        {
            Console.Write($"{User.Split('\\')[1]}@{Ip}> ");
            string line = Console.ReadLine();
            if (!string.IsNullOrEmpty(line))
            {
                foreach (var command in GetCommands())
                {
                    if (command.IsHandlable(line))
                    {
                        if (!command.Run(line))
                        {
                            return new CommandDoResult()
                            {
                                Command = command,
                                Input = line,
                                Result = CommandProcessResultType.IncorrectSyntax
                            };
                        }
                        else
                        {
                            return new CommandDoResult()
                            {
                                Command = command,
                                Input = line,
                                Result = CommandProcessResultType.Success
                            };
                        }
                    }
                }

                return new CommandDoResult()
                {
                    Command = null,
                    Input = line,
                    Result = CommandProcessResultType.CommandNotFound
                };
            }

            return new CommandDoResult()
            {
                Command = null,
                Input = null,
                Result = CommandProcessResultType.EmptyInput
            };
        }

        public bool IsEnabled()
            => !IsDisabled;

        public void Disable()
            => IsDisabled = true;

        public void Enable()
            => IsDisabled = false;

        public List<ICommandElement> GetCommands()
            => RegisteredCommands;

        public void AddCommand(ICommandElement element)
        {
            if(!RegisteredCommands.Contains(element))
            {
                RegisteredCommands.Add(element);
            }
        }

        private List<ICommandElement> Load()
        {
            return Assembly
                .GetEntryAssembly()
                .GetTypes()
                .Where(m => m.GetInterfaces().Contains(typeof(ICommandElement)))
                .Select(m => m.GetConstructor(Type.EmptyTypes).Invoke(null) as ICommandElement)
                .ToList();
        }
    }
}
