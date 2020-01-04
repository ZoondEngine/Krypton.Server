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
			Service = ClientFactory.CreateTcpClient("127.0.0.1", 8789);

			SubsBootstrap = new SubscribersBootstrap();
			SubsBootstrap.MountUp(this);
		}

		public bool Connect()
		{
			try
			{
				Service.Connect();
				//var task = Service.SendAndWait(new GetGuidPacket());
				//task.Wait();

				//var result = task.Result;
				//result.Connection.ChangeGuid(result.PacketContent.Convert<SetGuidPacket>().AcceptedGuid);

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

		public async Task<TcpNetworkData> SendAndWait(BaseNetworkable structure)
			=> await Service.SendAndWait(structure);
		public void Send(BaseNetworkable structure)
			=> Service.Send(structure);

		public TcpTunnel GetService()
			=> Service;
	}
}
