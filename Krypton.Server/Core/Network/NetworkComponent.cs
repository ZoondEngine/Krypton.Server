using Jareem.Network.Systems;
using Jareem.Network.Systems.Tcp.Server;
using Krypton.Server.Core.IO;
using Krypton.Server.Core.Log;
using Krypton.Support;
using Krypton.Support.CodeAnalyzer;
using System;

namespace Krypton.Server.Core.Network
{
	public class NetworkComponent : KryptonComponent<NetworkComponent>
	{
		private TcpServer m_service;
		private BootstrapHelper m_bootstrapper;
		private LogComponent m_log;

		public NetworkComponent()
		{
			m_service = ServerFactory.CreateTcpServer("194.87.109.35", 8789);
			m_log = LogComponent.Instance;
		}

		public void OnServerInitialized(IOMgr io)
		{
			using (var analyze = Analyze.Watch("Network initialization"))
			{
				io.GetPrint().Trace("Initiailizing network ...");

				m_bootstrapper = new BootstrapHelper(this);
				m_bootstrapper.MountAll();

				m_service.Start();
				m_log.Debug($"Network component has been started");
			}
		}

		public IOMgr GetIO()
			=> GetComponent<IOMgr>();

		public TcpServer GetService()
			=> m_service;

		public LogComponent GetLog()
			=> m_log;
	}
}
