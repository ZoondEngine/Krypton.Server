using Jareem.Network.Systems;
using Jareem.Network.Systems.Tcp.Server;
using Krypton.Server.Core.IO;
using Krypton.Support;
using Krypton.Support.CodeAnalyzer;

namespace Krypton.Server.Core.Network
{
	public class NetworkComponent : KryptonComponent<NetworkComponent>
	{
		private TcpServer Service;
		private BootstrapHelper Bootstrap;

		public NetworkComponent()
		{
			Service = ServerFactory.CreateTcpServer("194.87.109.35", 8789);
		}

		public void OnServerInitialized(IOMgr io)
		{
			using (var analyze = Analyze.Watch("Network initialization"))
			{
				io.GetPrint().Trace("Initiailizing network ...");

				Bootstrap = new BootstrapHelper(this);
				Bootstrap.MountAll();

				Service.Start();
			}
		}

		public IOMgr GetIO()
			=> GetComponent<IOMgr>();

		public TcpServer GetService()
			=> Service;
	}
}
