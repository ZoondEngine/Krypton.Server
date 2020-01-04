using Krypton.Network.Cryptography;
using Krypton.Network.Details;
using Krypton.Network.Packetize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatsonTcp;

namespace Krypton.Network.Systems
{
	public class TcpServer : BaseTcp
	{
		private WatsonTcpServer Service;

		public TcpServer(string ip, int port, bool debug = true)
			=> Initialize(ip, port, debug);

		public override void Initialize(string ip, int port, bool debug = true)
		{
			Service = new WatsonTcpServer(ip, port);
			SessionList = new List<TcpSession>();

			Service.Debug = debug;
			Service.ClientConnected = OnClientConnected;
			Service.ClientDisconnected = OnClientDisconnected;
			Service.MessageReceived = OnMessageReceived;

			Crypto = new Rijndael();
		}

		public WatsonTcpServer GetService()
			=> Service;

		public async void StartAsync()
		{
			await Service.StartAsync();
		}

		public void Start()
		{
			Service.Start();
		}

#pragma warning disable CS1998
		private async Task OnClientConnected(string from)
		{
			Guid uuid = Guid.NewGuid();
			if (SessionList.Exists((x) => x.NetworkIdentifier == uuid.ToString()))
			{
				do
				{
					uuid = Guid.NewGuid();
				}
				while (SessionList.Exists((x) => x.NetworkIdentifier == uuid.ToString()));
			}

			var session = new TcpSession(from, uuid.ToString());
			if (SessionList.Exists((x) => x.GetInternalNetwork() == from))
			{
				SessionList.Remove(SessionList.FirstOrDefault((x) => x.GetInternalNetwork() == from));
			}

			SessionList.Add(session);
			CallConnected(session);
		}

		private async Task OnClientDisconnected(string from, DisconnectReason reason)
		{
			var session = GetSessionByInternal(from);
			if (session != null)
			{
				SessionList.Remove(session);
			}

			CallDisconnected(session, reason);
		}

		private async Task OnMessageReceived(string from, byte[] data)
		{
			var session = GetSessionByInternal(from);
			if (session != null)
			{
				string encrypted = Encoding.UTF8.GetString(data);
				string decrypted = Crypto.Decrypt(encrypted);

				BaseNetworkable packet = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseNetworkable>(decrypted);
				CallReceived(session, packet);
			}
		}
#pragma warning restore CS1998

		public void Send(BaseNetworkable packet, TcpSession to)
		{
			var encrypted = Crypto.Encrypt(packet.ToNetwork());
			Service.Send(to.GetInternalNetwork(), encrypted);
		}

		public async Task SendAsync(BaseNetworkable packet, TcpSession to)
		{
			var encrypted = Crypto.Encrypt(packet.ToNetwork());
			await Service.SendAsync(to.GetInternalNetwork(), encrypted);
		}
	}
}
