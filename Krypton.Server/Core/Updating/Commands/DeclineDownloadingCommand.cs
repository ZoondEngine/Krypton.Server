using Krypton.Server.Core.IO.Contracts;
using System;

namespace Krypton.Server.Core.Updating.Commands
{
	public class DeclineDownloadingCommand : ICommandElement
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int Level { get; set; }

		public DeclineDownloadingCommand()
		{
			Name = "Updating decline helper";
			Description = "Command for decline/accept downloading hack";
			Level = 1;
		}

		public string GetHelp()
			=> ".upd decline 0";

		public bool IsHandlable(string line)
			=> line.Contains(".upd decline");

		public bool Run(string line)
		{
			var splitted = line.Split(' ');
			if(splitted.Length >= 3)
			{
				if(int.TryParse(splitted[2], out int is_decline))
				{
					var log = Log.LogComponent.Instance;

					var io = IO.IOMgr.Instance.GetPrint();
					var updater = UpdatingComponent.Instance;
					if(is_decline == 1)
					{
						updater.IsDeclineDownloadHack = true;

						io.Success("Dll downloading successfully declined");
						log.Trace("Dll downloading successfully declined");

						return true;
					}
					else if(is_decline == 0)
					{
						updater.IsDeclineDownloadHack = false;

						io.Success("Dll downloading successfully allowed");
						log.Trace("Dll downloading successfully allowed");

						return true;
					}
					else
					{
						return false;
					}
				}
			}

			return false;
		}
	}
}
