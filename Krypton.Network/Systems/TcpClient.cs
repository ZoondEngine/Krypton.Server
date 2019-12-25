using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatsonTcp;

namespace Krypton.Network.Systems
{
    public class TcpClient
    {
        private WatsonTcpClient Service;

        public TcpClient(string ip, int port)
        {
            Service = new WatsonTcpClient(ip, port);
            Service.ServerConnected = 
        }

        private async Task 
    }
}
