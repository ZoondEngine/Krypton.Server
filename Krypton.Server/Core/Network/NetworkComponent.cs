using Jareem.Network.Systems;
using Jareem.Network.Systems.Tcp.Server;
using Krypton.Server.Core.IO;
using Krypton.Server.Core.Log;
using Krypton.Server.Core.Network.Configuration;
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
		private NetworkConfiguration m_config;

		public NetworkComponent()
		{
			Ini.IniComponent.Instance.Load();
			m_config = Ini.IniComponent.Instance.GetByName("network-native-configuration").As<NetworkConfiguration>();

			//OnServerInitialized(IOMgr.Instance);
		}

		public void OnServerInitialized(IOMgr io)
		{
			using (var analyze = Analyze.Watch("Network initialization"))
			{
				io.GetPrint().Trace("Initiailizing network ...");

				m_service = ServerFactory.CreateTcpServer(m_config.Read<string>("host"), m_config.Read<int>("port"));
				m_log = LogComponent.Instance;

				m_bootstrapper = new BootstrapHelper(this);
				m_bootstrapper.MountAll();

				//m_service.Start();
				m_service.Start();
				m_log.Debug($"Network component has been started");
			}
		}

		public NetworkConfiguration GetConfig()
			=> m_config;

		public IOMgr GetIO()
			=> GetComponent<IOMgr>();

		public TcpServer GetService()
			=> m_service;

		public LogComponent GetLog()
			=> m_log;
	}
}
