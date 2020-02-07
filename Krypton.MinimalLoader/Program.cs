using Jareem.Network.Packets.KeyAuth;
using Krypton.MinimalLoader.Core.Hardware;
using Krypton.MinimalLoader.Core.Http;
using Krypton.MinimalLoader.Core.Native;
using Krypton.MinimalLoader.Core.Network;
using Krypton.MinimalLoader.Core.Security;
using System;
using System.Diagnostics;

namespace Krypton.MinimalLoader
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Write("Security checking ... \t\t");
			if (CheckSecurity(out string msg))
			{
				Console.Title = GenerateRandomWindowName();

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

								RunProcessAndInject(packet.TempDownloadString);
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

			Console.ResetColor();
			Console.WriteLine("Press any key to continue ...");
			Console.ReadKey();
		}

		private static void RunProcessAndInject(string path)
		{
			Console.WriteLine("Please wait ...");

			var process = new Process();
			process.StartInfo.FileName = "notepad";
			process.StartInfo.UseShellExecute = true;
			process.StartInfo.Verb = "runas";
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process.StartInfo.ErrorDialog = false;
			process.StartInfo.CreateNoWindow = false;
			process.Start();

			var injection = InjectionComponent.Instance;
			if (injection.SetupInjection(path, "notepad", process).Inject())
			{
				process.WaitForExit(3000);
				var exited = process.HasExited;

				if (!exited)
				{
					Console.WriteLine("Done. Close loader and start the game");
					Console.ReadKey();

					//TODO: normal exit log
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("ERROR");
					Console.WriteLine("Error message: Check your antiviruses and try again.");
					Console.ResetColor();

					Console.WriteLine("Press any key to continue");
					Console.ReadKey();
				}

				Console.ReadKey();
				Environment.Exit(0);
			}
			else
			{
				Console.WriteLine("IDI NAHUY");
				Console.ReadKey();
			}
		}

		private static string GenerateRandomWindowName(int length = 56)
		{
			string alphabet = "qwertyuiopasdfghjklzxcvbnm,.QWERTYUIOASDFGHJKLZXCVBNM1234567890";
			string generated = "";

			var rand = new Random();
			for(var i = 0; i < length; i++)
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
