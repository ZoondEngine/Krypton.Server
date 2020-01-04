using System;

namespace Krypton.Network.Packetize.Implemented
{
    public class AuthRequest : BaseNetworkable
    {
        public string HWID { get; set; }
        public string Key  { get; set; }

        public AuthRequest()
        {
            Identifier = (int)Packets.AuthRequest;
        }
    }
}
