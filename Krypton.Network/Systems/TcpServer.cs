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
    public delegate void DisconnectionDelegate(TcpSession session);
    public delegate void ReceiveDelegate(TcpServer session, BaseNetworkable data);

    public class TcpServer
    {
        private WatsonTcpServer Service;
        private List<TcpSession> SessionList;

        private event ConnectionDelegate OnConnected;
        private event DisconnectionDelegate OnDisconnected;
        private event ReceiveDelegate OnReceived;

        public TcpServer(string ip, int port, bool debug = true)
        {
            Service = new WatsonTcpServer(ip, port);

            Service.Debug = debug;
            Service.ClientConnected = OnClientConnected;
            Service.ClientDisconnected = OnClientDisconnected;
            Service.MessageReceived = OnMessageReceived;
        }

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

        private async Task OnClientConnected(string from)
        {

        }

        private async Task OnClientDisconnected(string from, DisconnectReason reason)
        {

        }

        private async Task OnMessageReceived(string from, byte[] data)
        {

        }
    }
}
