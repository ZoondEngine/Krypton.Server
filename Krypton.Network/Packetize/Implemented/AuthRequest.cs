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

        public override void Parse(string input)
        {
            var splitted = input.Split(DELIMETER);
            if(splitted.Length > 3)
            {
                HWID = splitted[1];
                Key  = splitted[2];
            }
        }

        public override string ToNetwork()
        {
            throw new NotImplementedException();
        }
    }
}
