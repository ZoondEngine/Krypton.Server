using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.Network.Cryptography.Contracts
{
    public interface ICryptoProvider
    {
        string Decrypt(string input);
        string Encrypt(string input);
    }
}
