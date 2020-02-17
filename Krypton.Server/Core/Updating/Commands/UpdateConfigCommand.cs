using Krypton.Server.Core.IO.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.Server.Core.Updating.Commands
{
	public class UpdateConfigCommand : ICommandElement
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int Level { get; set; }

		public UpdateConfigCommand()
		{
			Name = "Updating config reloader";
			Description = "Command for reloading server updating configuration";
			Level = 1;
		}

		public string GetHelp()
			=> ".upd reload [/w]";

		public bool IsHandlable(string line)
			=> line.ToLower().Contains(".upd reload");

		public bool Run(string line)
		{
			var updating_component = UpdatingComponent.Instance;
			var splitted = line.Split(' ');
			if(splitted.Length == 3)
			{
				if(splitted[2].ToLower() == "/w")
				{
					updating_component.ReloadSettings(true);
					return true;
				}
			}
			else
			{
				if(splitted.Length == 2)
				{
					updating_component.ReloadSettings(false);
					return true;
				}
			}

			return false;
		}
	}
}
