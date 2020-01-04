using Krypton.Network.Cryptography;
using Krypton.Network.Details;
using Krypton.Network.Packetize;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WatsonTcp;

namespace Krypton.Network.Systems
{
	public class TcpClient : BaseTcp
	{
		private WatsonTcpClient Service;

		public TcpClient(string ip, int port)
			=> Initialize(ip, port, true);

		public override void Initialize(string ip, int port, bool debug = false)
		{
			Service = new WatsonTcpClient(ip, port);
			SessionList = new List<TcpSession>();

			Service.Debug = debug;
			Service.ServerConnected = OnServerConnected;
			Service.ServerDisconnected = OnServerDisconnected;
			Service.MessageReceived = OnMessageReceived;

			Crypto = new Rijndael();
		}

		public void Start()
		{
			Service.Start();
		}

		public void Send(BaseNetworkable packet)
		{
			var encrypted = Crypto.Encrypt(packet.ToNetwork());
			Service.Send(encrypted);
		}

		public async Task SendAsync(BaseNetworkable packet)
		{
			var encrypted = Crypto.Encrypt(packet.ToNetwork());
			await Service.SendAsync(encrypted);
		}

#pragma warning disable CS1998
		private async Task OnServerConnected()
		{
			CallConnected(null);
		}

		private async Task OnServerDisconnected()
		{
			CallDisconnected(null, DisconnectReason.Normal);
		}

		private async Task OnMessageReceived(byte[] data)
		{
			string encrypted = Encoding.UTF8.GetString(data);
			string decrypted = Crypto.Decrypt(encrypted);

			BaseNetworkable packet = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseNetworkable>(decrypted);
			CallReceived(null, packet);
		}
#pragma warning restore CS1998
	}
}
