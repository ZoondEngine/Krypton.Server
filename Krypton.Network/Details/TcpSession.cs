using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.Network.Details
{
    public class TcpSession
    {
        public string Ip;
        public int Port;
        public string NetworkIdentifier;

        public TcpSession(string ip_port, string identifier)
        {
            var splitted = ip_port.Split(':');
            if(splitted.Length > 2)
            {
                if(int.TryParse(splitted[1], out int port))
                {
                    Ip = splitted[0];
                    Port = port;
                    NetworkIdentifier = identifier;
                }
            }
        }

        public string GetInternalNetwork()
            => Ip + ":" + Port;
    }
}
