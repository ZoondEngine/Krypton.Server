using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.Server
{
    class ServerEntry
    {
        private static ServerBootstrap BootstrapMgr;

        static void Main(string[] args)
        {
            BootstrapModules();
            Console.ReadKey();
        }

        private static void BootstrapModules()
        {
            BootstrapMgr = new ServerBootstrap();
            BootstrapMgr.Boot();
        }
    }
}
