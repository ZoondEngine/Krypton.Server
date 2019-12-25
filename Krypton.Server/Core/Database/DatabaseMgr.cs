using Krypton.Server.Core.IO;
using Krypton.Support;
using Krypton.Support.CodeAnalyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.Server.Core.Database
{
    public class DatabaseMgr : KryptonComponent<DatabaseMgr>
    {
        private IOMgr IO;

        public void OnServerInitialized(IOMgr io)
        {
            using(Analyze.Watch("Database modules initialization"))
            {
                IO = io;
                var cmds = Commands.CommandsMap.DatabaseCommands;

                IO.GetPrint().Trace("Database module successfully intended.");
                IO.GetPrint().Trace($"Database added '{cmds.Count}' implemented commands");
                IO.GetPrint().Trace("User '.db help' for getting database commands list");
            }
        }

        public Contexts.KeysContext GetKeysContext()
            => new Contexts.KeysContext();
    }
}
