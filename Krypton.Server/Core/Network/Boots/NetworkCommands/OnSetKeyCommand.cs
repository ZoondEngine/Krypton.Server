using Jareem.Network.Packets;
using Jareem.Network.Packets.KeyAuth;
using Jareem.Network.Systems.Tcp.Observeable.Providers.TcpReceiving;
using Jareem.Support.Implements.Observer.Base;
using System;
using System.IO;
using System.Linq;

namespace Krypton.Server.Core.Network.Boots.NetworkCommands
{
	public class OnSetKeyCommand : IObserver<BaseData>
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
			string message = "";
			string temp_download = "null";
			bool result = false;
			DateTime remaining = DateTime.MinValue;
			var rec = value.As<TcpNetworkData>();
			if (rec.PacketContent.Identifier == (int)Packets.GetKeyAuth)
			{
				var packet = rec.PacketContent.Convert<GetKeyAuth>();
				if (packet.IsValid())
				{
					var keys_context = Database.DatabaseMgr.Instance.GetKeysContext();
					var db_key = keys_context.Keys.FirstOrDefault((x) => x.Value.ToLower() == packet.Key.ToLower());
					if (db_key != null)
					{
						var parent_packet = keys_context.KeyPackets.FirstOrDefault((x) => x.Identifier == db_key.PacketId);
						if (parent_packet != null)
						{
							if (!db_key.IsBlocked() & !parent_packet.IsBlocked())
							{
								if (db_key.RegionCode.ToLower() == packet.LocaleShort.ToLower())
								{
									if (!Updating.UpdatingComponent.Instance.IsDeclineDownloadHack)
									{
										var between = DateTime.Now.Day - packet.ActivateDate.Day;
										if (between <= 1 && between >= -1)
										{
											if (db_key.Hardware == null | db_key.Hardware == "")
											{
												if (db_key.EndAt == null & db_key.ActivatedAt == null)
												{
													db_key.ActivatedAt = packet.ActivateDate;
													db_key.EndAt = packet.ActivateDate.AddDays(db_key.Days);
												}

												db_key.Hardware = packet.Hardware;

												result = true;
												temp_download = "http://control.kryptonware.xyz/storage/storage/app/updated/loader.exe";
												remaining = db_key.EndAt.Value;

												keys_context.SaveChanges();
											}
											else
											{
												if(db_key.Hardware == packet.Hardware)
												{
													result = true;
													temp_download = "http://control.kryptonware.xyz/storage/storage/app/updated/loader.exe";
													remaining = db_key.EndAt.Value;
												}
												else
												{
													NetworkComponent.Instance.GetLog().Error($"HACKING ATTEMPT! Code 0x0002(HARDWARE INVALID). HARDWARE: {packet.Hardware} - KEY: {packet.Key}");
													message = "Key or hardware identifier doesn't match";
												}
											}
										}
										else
										{
											NetworkComponent.Instance.GetLog().Error($"HACKING ATTEMPT! Code 0x0001(CHANGE DATETIME). HARDWARE: {packet.Hardware} - KEY: {packet.Key}");
											
											message = "Unknown time difficult exception";
											temp_download = "nil";
										}
									}
									else
									{
										message = "Krypton updating now. Please try again later";
									}
								}
								else
								{
									if(db_key.RegionCode.ToLower() == "ru")
									{
										if(packet.LocaleShort.ToLower() == "be" || packet.LocaleShort.ToLower() == "uk")
										{
											if (!Updating.UpdatingComponent.Instance.IsDeclineDownloadHack)
											{
												var between = DateTime.Now.Day - packet.ActivateDate.Day;
												if (between <= 1 && between >= -1)
												{
													if (db_key.Hardware == null | db_key.Hardware == "")
													{
														if (db_key.EndAt == null & db_key.ActivatedAt == null)
														{
															db_key.ActivatedAt = packet.ActivateDate;
															db_key.EndAt = packet.ActivateDate.AddDays(db_key.Days);
														}

														db_key.Hardware = packet.Hardware;

														result = true;
														temp_download = "http://control.kryptonware.xyz/storage/storage/app/updated/loader.exe";
														remaining = db_key.EndAt.Value;

														keys_context.SaveChanges();
													}
													else
													{
														if (db_key.Hardware == packet.Hardware)
														{
															result = true;
															temp_download = "http://control.kryptonware.xyz/storage/storage/app/updated/loader.exe";
															remaining = db_key.EndAt.Value;
														}
														else
														{
															NetworkComponent.Instance.GetLog().Error($"HACKING ATTEMPT! Code 0x0002(HARDWARE INVALID). HARDWARE: {packet.Hardware} - KEY: {packet.Key}");
															message = "Key or hardware identifier doesn't match";
														}
													}
												}
												else
												{
													NetworkComponent.Instance.GetLog().Error($"HACKING ATTEMPT! Code 0x0001(CHANGE DATETIME). HARDWARE: {packet.Hardware} - KEY: {packet.Key}");

													message = "Unknown time difficult exception";
													temp_download = "nil";
												}
											}
											else
											{
												message = "Krypton updating now. Please try again later";
											}
										}
										else
										{
											message = "Incorrect country for key or your contry market not supported";
										}
									}
									else
									{
										message = "Incorrect country for key or your contry market not supported";
									}
								}
							}
							else
							{
								message = "Key is locked";
							}
						}
						else
						{
							message = "License identifier not found";
						}
					}
					else
					{
						message = "Key not found";
					}
				}
			}

			if (rec.Connection.IsConnected)
			{

				NetworkComponent.Instance.GetService().Send(
					new SetKeyAuth()
					{
						Result = result,
						RemainingTime = remaining,
						TempDownloadString = temp_download,
						Message = message
					},
					rec.Connection);
			}
			else
			{
				throw new IOException("Transport protocol has been closed while server calculating key data");
			}
		}
	}
}
