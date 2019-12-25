using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.Network.Packetize
{
    public abstract class BaseNetworkable
    {
        public int Identifier { get; set; }

        public abstract void Parse(string input);

        public T Convert<T>() where T : BaseNetworkable
            => (T)this;
    }
}
