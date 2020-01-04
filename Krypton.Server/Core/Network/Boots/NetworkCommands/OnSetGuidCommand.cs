using Jareem.Network.Packets;
using Jareem.Network.Systems.Tcp.Observeable.Providers.TcpReceiving;
using Jareem.Support.Implements.Observer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.Server.Core.Network.Boots.NetworkCommands
{
	public class OnSetGuidCommand : IObserver<BaseData>
	{
		public void OnCompleted()
		{

		}

		public void OnError(Exception error)
		{

		}

		public void OnNext(BaseData value)
		{
			var rec = value.As<TcpNetworkData>();
			if (rec.PacketContent.Identifier == (int)Packets.GetGuid)
			{
				NetworkComponent.Instance.GetService().Send(new SetGuidPacket() { AcceptedGuid = rec.Connection.Identifier }, rec.Connection);
			}
		}
	}
}
