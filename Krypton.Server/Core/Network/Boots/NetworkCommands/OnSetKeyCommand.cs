﻿using Jareem.Network.Packets;
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
			//var print = IO.IOMgr.Instance.GetPrint();
			//print.Error($"Internal error catched while process SetKeyCommand( ... ): \n{error.ToString()}");
			Console.WriteLine($"Internal error catched while process SetKeyCommand( ... ): \n{error.ToString()}");
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
					//Console.WriteLine("valid");
					var keys_context = Database.DatabaseMgr.Instance.GetKeysContext();
					var db_key = keys_context.Keys.FirstOrDefault((x) => x.Value.ToLower() == packet.Key.ToLower());
					if (db_key != null)
					{
						//Console.WriteLine("key not null");
						var parent_packet = keys_context.KeyPackets.FirstOrDefault((x) => x.Identifier == db_key.PacketId);
						if (parent_packet != null)
						{
							//Console.WriteLine("parent packet");
							if (!db_key.IsBlocked() & !parent_packet.IsBlocked())
							{
								//Console.WriteLine("not blocked");
								if (db_key.RegionCode.ToLower() == packet.LocaleShort.ToLower() || db_key.Enabled == 0)
								{
									//if(db_key.EndAt >= packet.ActivateDate || db_key.EndAt >= DateTime.Now)
									//{
										if (!Updating.UpdatingComponent.Instance.IsDeclineDownloadHack)
										{
											var between = DateTime.Now.Day - packet.ActivateDate.Day;
											if (between <= 1 && between >= -1)
											{
												//Console.WriteLine("between");
												if (db_key.Hardware == null | db_key.Hardware == "")
												{
													//Console.WriteLine("!hardware");
													if (db_key.EndAt == null & db_key.ActivatedAt == null)
													{
														db_key.ActivatedAt = packet.ActivateDate;
														db_key.EndAt = packet.ActivateDate.AddDays(db_key.Days);
													}

													db_key.Hardware = packet.Hardware;

													var config = Updating.UpdatingComponent.Instance.GetConfig();

													result = true;
													//temp_download = "http://control.kryptonware.xyz/storage/storage/app/updated/loader.exe";
													temp_download = config.Read<string>("link", "dll");
													remaining = db_key.EndAt.Value;

													keys_context.SaveChanges();
												}
												else
												{
													//Console.WriteLine("hardware");
													if (db_key.Hardware == packet.Hardware)
													{
													if (db_key.EndAt > packet.ActivateDate || db_key.EndAt > DateTime.Now)
													{
														//Console.WriteLine("compared. Done");
														result = true;
														var config = Updating.UpdatingComponent.Instance.GetConfig();
														//temp_download = "http://control.kryptonware.xyz/storage/storage/app/updated/loader.exe";
														temp_download = config.Read<string>("link", "dll");
														remaining = db_key.EndAt.Value;
													}
													else
													{
														message = "Key expired";
													}
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
									//}
									//else
									//{
									//	message = "Key expired";
									//}
								}
								else
								{
									//Console.WriteLine("ru1");
									if (db_key.RegionCode.ToLower() == "ru")
									{
										//Console.WriteLine("ru2");
										if (packet.LocaleShort.ToLower() == "be" || packet.LocaleShort.ToLower() == "uk")
										{
											//Console.WriteLine("be or uk");
											if (!Updating.UpdatingComponent.Instance.IsDeclineDownloadHack)
											{
												//Console.WriteLine("allowed");
												var between = DateTime.Now.Day - packet.ActivateDate.Day;
												if (between <= 1 && between >= -1)
												{
													//Console.WriteLine("between");
													if (db_key.Hardware == null | db_key.Hardware == "")
													{
														//Console.WriteLine("!hardware");
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
														//Console.WriteLine("hardware");
														if (db_key.Hardware == packet.Hardware)
														{
															//Console.WriteLine("compared.done");
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
}
