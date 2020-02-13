using Jareem.Network.Detail;
using Jareem.Network.Packets;
using Krypton.MinimalLoader.Core.Network;
using Krypton.Support;
using System.Diagnostics;
using System.IO;

namespace Krypton.MinimalLoader.Core.Updating
{
	public class UpdatingComponent : KryptonComponent<UpdatingComponent>
	{
		private NetworkComponent NetworkInstance { get; set; }
		private Version CurrentVersion { get; set; }
		private string DownloadString { get; set; }
		private Version RemoteVersion { get; set; }

		public UpdatingComponent()
		{
			NetworkInstance = NetworkComponent.Instance;
			CurrentVersion = new Version("1.0.483.6");
		}

		public bool IsNeededUpdating()
		{
			var result = NetworkInstance.SendAndWait(new GetUpdatePacket()
			{
				Hardware = Hardware.HardwareComponent.Instance.GetHardwareId(),
				Version = CurrentVersion.ToString()
			});

			if(result.PacketContent.Identifier == (int)Packets.SetUpdate)
			{
				var packet = result.PacketContent.Convert<SetUpdatePacket>();

				if (packet.ErrorOccured)
				{
					System.Console.ForegroundColor = System.ConsoleColor.Red;
					System.Console.WriteLine($"ERROR");
					System.Console.WriteLine($"Error message: Abnormal updating. {packet.ErrorMessage}");
					System.Console.ResetColor();

					return false;
				}
				else
				{
					if(packet.IsNeeded)
					{
						DownloadString = packet.DownloadString;
						RemoteVersion = Version.Parse(packet.NewVersion);

						return true;
					}
				}
			}

			return false;
		}

		public void DownloadUpdater()
		{
			if(Http.HttpComponent.Instance.Download(DownloadString, out string temp_path))
			{
				var new_path = "KryptonWare_" + RemoteVersion.ToString().Replace('.', '_') + ".exe";
				File.Move(temp_path, new_path);

				Process.Start(new_path);
			}
			else
			{
				System.Console.ForegroundColor = System.ConsoleColor.Red;
				System.Console.WriteLine($"ERROR");
				System.Console.WriteLine("Error message: Abnormal updating. Please try again later");
				System.Console.ResetColor();
			}
		}
	}
}
