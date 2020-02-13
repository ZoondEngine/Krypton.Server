using Krypton.Server.Core.Ini.Contracts;
using Krypton.Server.Core.Ini.Details;
using System.IO;

namespace Krypton.Server.Core.Database.Configuration
{
	public class DatabaseConfiguration  : IModuleConfiguration
	{
		private string m_file_path;
		private IniFile m_cfg_file;

		public DatabaseConfiguration()
		{ }

		public T Read<T>(string key, string section = "general")
			=> Ini.IniComponent.Instance.GetBySectionValue<T>(GetNativeIni(), section, key);

		public static void BuildDefault()
		{
			var path = Path.Combine(GlobalSettings.Configuration.ConfigurationDirectory, GlobalSettings.Configuration.DatabaseConfigurationFile);
			var ini_file = Ini.IniComponent.Instance.CreateIni();

			ini_file["general"]["host"] = "localhost";
			ini_file["general"]["user"] = "root";
			ini_file["general"]["password"] = "";
			ini_file["general"]["database"] = "evilcorp";

			ini_file.Save(path);
		}

		public string GetName()
			=> "database-native-configuration";

		public string GetPath()
			=> m_file_path;

		public IniFile GetNativeIni()
			=> m_cfg_file;

		public void Default()
			=> BuildDefault();

		public void Load()
		{
			m_file_path = Path.Combine(GlobalSettings.Configuration.ConfigurationDirectory, GlobalSettings.Configuration.DatabaseConfigurationFile);
			if (!File.Exists(GetPath()))
			{
				BuildDefault();
			}

			m_cfg_file = Ini.IniComponent.Instance.OpenFile(GetPath());
		}

		public T As<T>() where T : IModuleConfiguration
			=> (T)(IModuleConfiguration)this;
	}
}
