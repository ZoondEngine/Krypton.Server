using Jareem.Network.Packets;
using Jareem.Network.Packets.Journaling;
using Jareem.Network.Systems.Tcp.Observeable.Providers.TcpReceiving;
using Jareem.Support.Implements.Observer.Base;
using System;

namespace Krypton.Server.Core.Network.Boots.NetworkCommands
{
	public class OnSetJournalCommand : IObserver<BaseData>
	{
		public void OnCompleted()
		{

		}

		public void OnError(Exception error)
		{
			var print = IO.IOMgr.Instance.GetPrint();
			print.Error($"Internal error catched while process SetKeyCommand( ... ): \n{error.ToString()}");
		}

		public void OnNext(BaseData value)
		{
			var rec = value.As<TcpNetworkData>();
			if (rec.PacketContent.Identifier == (int)Packets.GetJournal)
			{
				var journal_packet = rec.PacketContent.Convert<GetJournalPacket>();
				if(journal_packet != null)
				{
					
				}
			}
		}
	}
}
