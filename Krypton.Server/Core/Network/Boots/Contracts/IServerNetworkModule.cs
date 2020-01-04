using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.Server.Core.Network.Boots.Contracts
{
	public interface IServerNetworkModule
	{
		void MountUp(NetworkComponent network);
		void MountDown(NetworkComponent network);
	}
}
