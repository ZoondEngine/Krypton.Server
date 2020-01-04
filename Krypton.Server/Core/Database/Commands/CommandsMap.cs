using Krypton.Server.Core.IO.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.Server.Core.Database.Commands
{
    public static class CommandsMap
    {
        public static List<ICommandElement> DatabaseCommands = new List<ICommandElement>()
        {
            new KeyPacketsInfoCommand(),
            new KeyInfoCommand(),
						new KeysClearOldCommand(),
        };
    }
}
