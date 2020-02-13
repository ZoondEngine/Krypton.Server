using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.Server.Core
{
	public static class GlobalSettings
	{
		public static class Configuration
		{
			public static string ConfigurationDirectory = "internal\\configuration\\";
			public static string DatabaseConfigurationFile = "database.cfg";
			public static string NetworkConfigurationFile = "network.cfg";
			public static string UpdatingComfigurationFile = "updates.cfg";
		}
	}
}
