using Krypton.Network.Cryptography.Contracts;
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
	public abstract class BaseTcp
	{
		public delegate void ConnectionDelegate(TcpSession session);
		public delegate void DisconnectionDelegate(TcpSession session, DisconnectReason reason);
		public delegate void ReceiveDelegate(TcpSession session, BaseNetworkable data);

		protected event ConnectionDelegate OnConnected;
		protected event DisconnectionDelegate OnDisconnected;
		protected event ReceiveDelegate OnReceived;


		protected List<TcpSession> SessionList;
		protected ICryptoProvider Crypto;

		public abstract void Initialize(string ip, int port, bool debug = true);
		public TcpSession GetSessionByInternal(string internal_uid)
		=> SessionList.FirstOrDefault((x) => x.GetInternalNetwork() == internal_uid);

		protected void CallConnected(TcpSession session)
			=> OnConnected?.Invoke(session);

		protected void CallDisconnected(TcpSession session, DisconnectReason reason)
			=> OnDisconnected?.Invoke(session, reason);

		protected void CallReceived(TcpSession session, BaseNetworkable data)
			=> OnReceived?.Invoke(session, data);

		#region Subscribes
		public void SubscribeConnected(ConnectionDelegate handler)
		{
			OnConnected += handler;
		}
		public void SubscribeDisconnected(DisconnectionDelegate handler)
		{
			OnDisconnected += handler;
		}
		public void SubscribeReceived(ReceiveDelegate handler)
		{
			OnReceived += handler;
		}

		public void UnsubscribeConnected(ConnectionDelegate handler)
		{
			if (OnConnected.GetInvocationList().Contains(handler))
			{
				OnConnected -= handler;
			}
		}
		public void UnsubscribeDisconnected(DisconnectionDelegate handler)
		{
			if (OnDisconnected.GetInvocationList().Contains(handler))
			{
				OnDisconnected -= handler;
			}
		}
		public void UnsubscribeReceived(ReceiveDelegate handler)
		{
			if (OnReceived.GetInvocationList().Contains(handler))
			{
				OnReceived -= handler;
			}
		}
		#endregion
	}
}
