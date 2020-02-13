using Krypton.Server.Core.IO;
using Krypton.Server.Core.Network.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatsonTcp;
using Krypton.Support;

namespace Krypton.Server.Core.NetworkWatson
{
	public class WatsonNetworkComponent// : KryptonComponent<WatsonNetworkComponent>
	{
		private WatsonTcpServer m_server;
		private NetworkConfiguration m_config;

		public WatsonNetworkComponent()
		{
			Ini.IniComponent.Instance.Load();
			m_config = Ini.IniComponent.Instance.GetByName("network-native-configuration").As<NetworkConfiguration>();
		}

		//public void OnServerInitialized(IOMgr io)
		//{
		//	m_server = new WatsonTcpServer(m_config.Read<string>("host"), m_config.Read<int>("port"));

		//	m_server.ClientConnected = ClientConnected;
		//	m_server.ClientDisconnected = ClientDisconnected;
		//	m_server.MessageReceived = MessageReceived;

		//	io.GetPrint().Success($"Server listened on: {m_config.Read<string>("host")}");
		//	m_server.Start();
		//}

		static async Task ClientConnected(string ipPort)
		{
			Console.WriteLine("Client connected: " + ipPort);
		}

		static async Task ClientDisconnected(string ipPort, DisconnectReason reason)
		{
			Console.WriteLine("Client disconnected: " + ipPort + ": " + reason.ToString());
		}

		static async Task MessageReceived(string ipPort, byte[] data)
		{
			string msg = "";
			if (data != null && data.Length > 0) msg = Encoding.UTF8.GetString(data);
			Console.WriteLine("Message received from " + ipPort + ": " + msg);
		}
	}
}
