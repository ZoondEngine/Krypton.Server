using Jareem.Network.Packets;
using Jareem.Network.Systems;
using Jareem.Network.Systems.Tcp.Client;
using Jareem.Network.Systems.Tcp.Observeable.Providers.TcpConnection;
using Jareem.Network.Systems.Tcp.Observeable.Providers.TcpReceiving;
using Krypton.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.MinimalLoader.Core.Network
{
	public class NetworkComponent : KryptonComponent<NetworkComponent>
	{
		private TcpTunnel Service;
		private SubscribersBootstrap SubsBootstrap;

		public NetworkComponent()
		{
			Service = ClientFactory.CreateTcpClient("194.87.109.35", 8789);
			//Service = ClientFactory.CreateTcpClient("127.0.0.1", 8789);

			SubsBootstrap = new SubscribersBootstrap();
			SubsBootstrap.MountUp(this);
		}

		public bool Connect()
		{
			try
			{
				Service.Connect();

				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}
		public void Disconnect()
			=> Service.Disconnect();
		public bool IsConnected()
			=> Service.IsConnected();
		public TcpConnection GetConnection()
			=> Service.GetConnection();

		public TcpNetworkData SendAndWait(BaseNetworkable structure)
			=> Service.SendAndWait(structure);
		public void Send(BaseNetworkable structure)
			=> Service.Send(structure);

		public TcpTunnel GetService()
			=> Service;
	}
}
