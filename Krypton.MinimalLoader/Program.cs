using Jareem.Network.Packets.KeyAuth;
using Krypton.MinimalLoader.Core.Hardware;
using Krypton.MinimalLoader.Core.Http;
using Krypton.MinimalLoader.Core.Native;
using Krypton.MinimalLoader.Core.Network;
using Krypton.MinimalLoader.Core.Security;
using System;

namespace Krypton.MinimalLoader
{
	class Program
	{
		static void Main(string[] args)
		{
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

					Console.Write("Enter your key: ");
					var key = Console.ReadLine();

					var hardware_id = hardware.GetHardwareId();
					var locale_code = hardware.GetLocale().GetLocaleCode();
					var locale_short = hardware.GetLocale().GetShortLocale();

					Console.Write("Authorize key ... \t\t");

					var task = net.SendAndWait(new GetKeyAuth() { Hardware = hardware_id, Key = key, LocaleCode = locale_code, LocaleShort = locale_short, ActivateDate = DateTime.Now });
					task.Wait();

					var result = task.Result;
					var packet = result.PacketContent.Convert<SetKeyAuth>();
					if (packet.Result)
					{
						ChangeColor(ConsoleColor.Green);
						Console.WriteLine("OK");
						Console.ResetColor();

						Console.Write("Waiting server information ... \t");

						try
						{
							var http = HttpComponent.Instance;
							if (http.Download(packet.TempDownloadString, out string path))
							{
								ChangeColor(ConsoleColor.Green);
								Console.WriteLine("OK");
								Console.ResetColor();

								Console.WriteLine($"Expiration date: {packet.RemainingTime}");

								var starter_component = new ProcessStarterComponent();
								var injection = InjectionComponent.Instance;
								while (true)
								{
									starter_component.Run("notepad.exe", true);
									if (injection.SetupInjection(path, "notepad", starter_component.GetProcess()).Inject())
									{
										if (starter_component.IsRunned())
										{
											break;
										}
									}

									starter_component.Stop();
								}

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
						if (packet.TempDownloadString == "nil")
						{
							//TODO: send log for attempt hack datetime logic
						}

						ChangeColor(ConsoleColor.Red);
						Console.WriteLine("ERROR");
						Console.WriteLine($"Error message: {packet.Message}");
						Console.ResetColor();
					}
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

			ChangeColor(ConsoleColor.Red);
			Console.WriteLine("Error message: Please solve your issues, restart loader and try again");

			Console.ResetColor();
			Console.WriteLine("Press any key to close");
			Console.ReadKey();
		}

		public static void WaitingAndClose(ProcessStarterComponent psc)
		{
			while (true)
			{
				//Ahuennaya Zaglushka 
			}
		}

		private static bool CheckSecurity(out string message)
			=> SecurityComponent.Instance.Check(out message);

		private static void ChangeColor(ConsoleColor color)
		{
			Console.ForegroundColor = color;
		}
	}
}
