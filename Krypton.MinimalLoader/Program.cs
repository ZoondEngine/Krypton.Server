using Jareem.Network.Packets;
using Jareem.Network.Packets.KeyAuth;
using Jareem.Network.Systems.Tcp.Observeable.Providers.TcpReceiving;
using Krypton.MinimalLoader.Core.Hardware;
using Krypton.MinimalLoader.Core.Http;
using Krypton.MinimalLoader.Core.Native;
using Krypton.MinimalLoader.Core.Network;
using Krypton.MinimalLoader.Core.Security;
using Krypton.MinimalLoader.Core.Updating;
using System;
using System.Threading;

namespace Krypton.MinimalLoader
{
	class Program
	{
		public static bool NextStage = false;
		public static TcpNetworkData Data = null;

		//public static string Result = "";
		//public static string Message = "";
		//public static string Expired = "";

		static void Main(string[] args)
		{
			Console.Title = GenerateRandomWindowName();

			Console.Write("Security checking ... \t\t");
			if (CheckSecurity(out string msg))
			{
				ChangeColor(ConsoleColor.Green);
				Console.WriteLine("OK");
				Console.ResetColor();

				Console.Write("Connecting... \t\t\t");
				var net = NetworkComponent.Instance;
				if (net.Connect())
				{
					ChangeColor(ConsoleColor.Green);
					Console.WriteLine($"OK");
					Console.ResetColor();

					var hardware = HardwareComponent.Instance;

					Console.Write("Checking updates ... \t\t");
					if (UpdatingComponent.Instance.IsNeededUpdating())
					{
						ChangeColor(ConsoleColor.Yellow);
						Console.WriteLine("UPDATING");
						Console.ResetColor();

						UpdatingComponent.Instance.DownloadUpdater();
						Environment.Exit(0);
					}
					else
					{
						ChangeColor(ConsoleColor.Green);
						Console.WriteLine("NORMAL");
						Console.ResetColor();
					}


					Console.Write("Enter your key: ");
					var key = Console.ReadLine();

					var hardware_id = hardware.GetHardwareId();
					var locale_code = hardware.GetLocale().GetLocaleCode();
					var locale_short = hardware.GetLocale().GetShortLocale();

					Console.Write("Authorize key ... \t\t");
					var reply = net.SendAndWait(new GetKeyAuth() { Hardware = hardware_id, Key = key, LocaleCode = locale_code, LocaleShort = locale_short, ActivateDate = DateTime.Now });
					
					Data = reply;
					Stage2();
				}
				else
				{
					ChangeColor(ConsoleColor.Red);
					Console.WriteLine($"ERROR");
					Console.WriteLine("Error message: Cannot initialize the network subsystem. Check your internet connection and try again");
					Console.ResetColor();
				}
			}
			else
			{
				ChangeColor(ConsoleColor.Red);
				Console.WriteLine("ERROR");
				Console.WriteLine($"Error message: {msg}");
			}

			Console.ResetColor();
			Console.WriteLine("Press any key to continue ...");
			Console.ReadKey();
		}

		private static void Stage2()
		{
			var result = Data;
			if (result != null)
			{
				if (result.PacketContent.Identifier == (int)Packets.SetKeyAuth)
				{
					var reply = result.PacketContent.Convert<SetKeyAuth>();
					if (reply.Result)
					{
						ChangeColor(ConsoleColor.Green);
						Console.WriteLine("OK");
						Console.ResetColor();

						Console.Write("Waiting server information ... \t");

						try
						{
							var http = HttpComponent.Instance;
							if (http.Download(reply.TempDownloadString, out string path))
							{
								ChangeColor(ConsoleColor.Green);
								Console.WriteLine("OK");
								Console.ResetColor();

								Console.WriteLine($"Expiration date: {reply.RemainingTime}");

								var starter_component = new ProcessStarterComponent();
								var injection = InjectionComponent.Instance;
								while (true)
								{
									starter_component.Run("notepad.exe", true);
									if (injection.SetupInjection(path, "notepad", starter_component.GetProcess()).Inject())
									//if(injection.SetupInjection(path, "explorer").Inject())
									{
										if (starter_component.IsRunned())
										{
											break;
										}
									}

									starter_component.Stop();
								}

								//if(injection.SetupInjection(path, "sihost").Inject())
								//{
								//	Console.WriteLine("Done");
								//	Console.ReadKey();
								//}
								if (starter_component.IsRunned())
								{
									WaitingAndClose(starter_component);
								}
								else
								{
									ChangeColor(ConsoleColor.Red);
									Console.WriteLine("ERROR");
									Console.WriteLine("Error message: Server technical failure. Try again later.");
									Console.ResetColor();
								}
							}
							else
							{
								ChangeColor(ConsoleColor.Red);
								Console.WriteLine("ERROR");
								Console.WriteLine($"Error message: Server technical failure. Try again later.");
								Console.ResetColor();
							}
						}
						catch (Exception)
						{
							ChangeColor(ConsoleColor.Red);
							Console.WriteLine("ERROR");
							Console.WriteLine($"Error message: Server technical failure. Try again later.");
							Console.ResetColor();
						}
					}
					else
					{
						if (reply.Message == "nil")
						{
							//TODO: send log for attempt hack datetime logic
						}

						ChangeColor(ConsoleColor.Red);
						Console.WriteLine("ERROR");
						Console.WriteLine($"Error message: {reply.Message}");
						Console.ResetColor();
					}
				}
				else
				{
					ChangeColor(ConsoleColor.Red);
					Console.WriteLine("ERROR");
					Console.WriteLine($"Error message: Internal loader error! 0x00000004");
					Console.ResetColor();
				}
			}
			else
			{
				ChangeColor(ConsoleColor.Red);
				Console.WriteLine("ERROR");
				Console.WriteLine($"Error message: Network error! 0x00000008");
				Console.ResetColor();
			}

			Console.ResetColor();
			Console.WriteLine("Press any key to continue ...");
			Console.ReadKey();
		}

		public static void WaitingAndClose(ProcessStarterComponent psc)
		{
			while (true)
			{
				//Ahuennaya Zaglushka 
			}
		}

		private static string GenerateRandomWindowName(int length = 56)
		{
			string alphabet = "qwertyuiopasdfghjklzxcvbnm,.QWERTYUIOASDFGHJKLZXCVBNM1234567890";
			string generated = "";

			var rand = new Random();
			for (var i = 0; i < length; i++)
			{
				generated += alphabet[rand.Next(0, alphabet.Length - 1)];
			}

			return generated;
		}

		private static bool CheckSecurity(out string message)
			=> SecurityComponent.Instance.Check(out message);

		private static void ChangeColor(ConsoleColor color)
		{
			Console.ForegroundColor = color;
		}
	}
}
