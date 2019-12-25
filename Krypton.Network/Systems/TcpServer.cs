using Krypton.Network.Cryptography;
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
    public delegate void ConnectionDelegate(TcpSession session);
    public delegate void DisconnectionDelegate(TcpSession session, DisconnectReason reason);
    public delegate void ReceiveDelegate(TcpSession session, BaseNetworkable data);

    public class TcpServer
    {
        private WatsonTcpServer Service;
        private List<TcpSession> SessionList;
        private ICryptoProvider Crypto;

        private event ConnectionDelegate OnConnected;
        private event DisconnectionDelegate OnDisconnected;
        private event ReceiveDelegate OnReceived;

        public TcpServer(string ip, int port, bool debug = true)
        {
            Service = new WatsonTcpServer(ip, port);
            SessionList = new List<TcpSession>();

            Service.Debug = debug;
            Service.ClientConnected = OnClientConnected;
            Service.ClientDisconnected = OnClientDisconnected;
            Service.MessageReceived = OnMessageReceived;

            Crypto = new Rijndael();
        }

        public async void StartAsync()
        {
            await Service.StartAsync();
        }

        public void Start()
        {
            Service.Start();
        }

        #region Subscribes
        public void SubscribeConnected(ConnectionDelegate handler)
        {
            if(!OnConnected.GetInvocationList().Contains(handler))
            {
                OnConnected += handler;
            }
        }
        public void SubscribeDisconnected(DisconnectionDelegate handler)
        {
            if (OnDisconnected.GetInvocationList().Contains(handler))
            {
                OnDisconnected += handler;
            }
        }
        public void SubscribeReceived(ReceiveDelegate handler)
        {
            if (!OnReceived.GetInvocationList().Contains(handler))
            {
                OnReceived += handler;
            }
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

#pragma warning disable CS1998
        private async Task OnClientConnected(string from)
        {
            Guid uuid = Guid.NewGuid();
            if(SessionList.Exists((x) => x.NetworkIdentifier == uuid.ToString()))
            {
                do
                {
                    uuid = Guid.NewGuid();
                }
                while (SessionList.Exists((x) => x.NetworkIdentifier == uuid.ToString()));
            }

            var session = new TcpSession(from, uuid.ToString());
            if(SessionList.Exists((x) => x.GetInternalNetwork() == from))
            {
                SessionList.Remove(SessionList.FirstOrDefault((x) => x.GetInternalNetwork() == from));
            }

            SessionList.Add(session);
            OnConnected?.Invoke(session);
        }

        private async Task OnClientDisconnected(string from, DisconnectReason reason)
        {
            var session = GetSessionByInternal(from);
            if(session != null)
            {
                SessionList.Remove(session);
            }

            OnDisconnected?.Invoke(session, reason);
        }

        private async Task OnMessageReceived(string from, byte[] data)
        {
            var session = GetSessionByInternal(from);
            if(session != null)
            {
                string encrypted = Encoding.UTF8.GetString(data);
                string decrypted = Crypto.Decrypt(encrypted);

                BaseNetworkable packet = new BaseNetworkable();

                //TODO: PARSING PACKETS!
                OnReceived?.Invoke(session, packet);
            }
        }
#pragma warning restore CS1998

        public TcpSession GetSessionByInternal(string internal_uid)
            => SessionList.FirstOrDefault((x) => x.GetInternalNetwork() == internal_uid);

        public void Send(BaseNetworkable packet, TcpSession to)
        {
            Service.Send(to.GetInternalNetwork(), packet.ToNetwork());
        }

        public async Task SendAsync(BaseNetworkable packet, TcpSession to)
        {
            await Service.SendAsync(to.GetInternalNetwork(), packet.ToNetwork());
        }
    }
}
