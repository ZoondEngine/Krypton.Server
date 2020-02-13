using Jareem.Network.Packets;
using Jareem.Network.Systems.Tcp.Observeable.Providers.TcpReceiving;
using Jareem.Support.Implements.Observer.Base;
using Krypton.Server.Core.Updating;
using System;
using System.IO;
using Ver = Jareem.Network.Detail.Version;

namespace Krypton.Server.Core.Network.Boots.NetworkCommands
{
	public class OnSetUpdateCommand : IObserver<BaseData>
	{
		public void OnCompleted()
		{

		}

		public void OnError(Exception error)
		{
			var print = IO.IOMgr.Instance.GetPrint();
			print.Error($"Internal error catched while process SetUpdateCommand( ... ): \n{error.ToString()}");
		}

		public void OnNext(BaseData value)
		{
			var rec = value.As<TcpNetworkData>();
			if (rec.PacketContent.Identifier == (int)Packets.GetUpdate)
			{
				bool err_occured = false;
				bool is_needed = false;
				string err_msg = "";
				string download_str = "";
				Ver new_version = Ver.Zero();

				var update_packet = rec.PacketContent.Convert<GetUpdatePacket>();
				if (update_packet != null)
				{
					var config = UpdatingComponent.Instance.GetConfig();
					if (Ver.TryParse(config.Read<string>("version", "loader"), out Ver retail))
					{
						if (Ver.TryParse(update_packet.Version, out Ver client_ver))
						{
							if (retail.IsGreaterThen(client_ver))
							{
								is_needed = true;
								download_str = config.Read<string>("link", "loader");
								new_version = retail;
							}
						}
						else
						{
							err_occured = true;
							err_msg = "Incorrect input version";
						}
					}
					else
					{
						err_occured = true;
						err_msg = "Incorrect version";
					}
				}
				else
				{
					err_occured = true;
					err_msg = "Invalid tunnel input";
				}

				if (rec.Connection.IsConnected)
				{
					NetworkComponent.Instance.GetService().Send(
						new SetUpdatePacket()
						{
							IsNeeded = is_needed,
							ErrorOccured = err_occured,
							ErrorMessage = err_msg,
							DownloadString = download_str,
							NewVersion = new_version.ToString()
						},
						rec.Connection);
				}
				else
				{
					throw new IOException("Transport protocol has been closed while server update logic");
				}
			}
		}
	}
}
