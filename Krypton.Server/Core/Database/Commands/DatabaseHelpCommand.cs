using Krypton.Server.Core.IO.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.Server.Core.Database.Commands
{
    public class DatabaseHelpCommand : ICommandElement
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }

        public DatabaseHelpCommand()
        {
            Name = ".db help";
            Description = "Command for getting database commands information";
            Level = 1;
        }

        public string GetHelp()
            => ".db help";

        public bool IsHandlable(string line)
            => line.Contains(".db help");

        public bool Run(string line)
        {
            using (var context = DatabaseMgr.Instance.GetKeysContext())
            {
                var print = IO.IOMgr.Instance.GetPrint();
                print.Trace("==========================================================================================");

                foreach (var cmd in CommandsMap.DatabaseCommands)
                {
                    print.Warning($" > Command: {cmd.Name}");
                    print.Trace($"   -- Usage: {cmd.GetHelp()}");
                    print.Trace($"   -- Description: {cmd.Description}");

                    if (cmd != CommandsMap.DatabaseCommands.Last())
                    {
                        print.Trace("------------------------------------------------------------------------------------------");
                    }
                }

                print.Trace("==========================================================================================");
            }

            return true;
        }
    }
}
