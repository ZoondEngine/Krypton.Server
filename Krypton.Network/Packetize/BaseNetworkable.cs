using System;

namespace Krypton.Network.Packetize
{
    public enum Packets
    {
        AuthRequest = 1,
        AuthResponse = 2,


    }

    public class BaseNetworkable
    {
        protected const char DELIMETER = ':';

        public int Identifier { get; set; }

        public virtual void Parse(string input)
        {
            throw new NotImplementedException();
        }
        public virtual string ToNetwork()
        {
            throw new NotImplementedException();
        }

        public T Convert<T>() where T : BaseNetworkable
            => (T)this;
    }
}
