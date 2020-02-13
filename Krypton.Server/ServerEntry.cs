using System;

namespace Krypton.Server
{
	class ServerEntry
	{
		private static ServerBootstrap BootstrapMgr = new ServerBootstrap();

		static void Main(string[] args)
		{
			BootstrapModules();
			Console.ReadKey();
		}

		private static void BootstrapModules()
		{
			//BootstrapMgr = new ServerBootstrap();

			//BootstrapMgr.Build();
			//BootstrapMgr.Boot();
		}
	}
}
