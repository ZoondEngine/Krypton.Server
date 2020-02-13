using Krypton.Server.Core.Ini.Contracts;
using Krypton.Server.Core.Ini.Details;
using System.IO;

namespace Krypton.Server.Core.Network.Configuration
{
	public class NetworkConfiguration : IModuleConfiguration
	{
		private string m_file_path;
		private IniFile m_cfg_file;

		public T As<T>() where T : IModuleConfiguration
			=> (T)(IModuleConfiguration)this;

		public void Default()
		{
			var path = Path.Combine(GlobalSettings.Configuration.ConfigurationDirectory, GlobalSettings.Configuration.NetworkConfigurationFile);
			var comp = Ini.IniComponent.Instance;
			var ini_file = comp.CreateIni();

			ini_file["general"]["host"] = "127.0.0.1";
			ini_file["general"]["port"] = 8789;

			ini_file.Save(path);
		}

		public string GetName()
			=> "network-native-configuration";

		public IniFile GetNativeIni()
			=> m_cfg_file;

		public string GetPath()
			=> m_file_path;

		public void Load()
		{
			m_file_path = Path.Combine(GlobalSettings.Configuration.ConfigurationDirectory, GlobalSettings.Configuration.NetworkConfigurationFile);
			if (!File.Exists(GetPath()))
			{
				Default();
			}

			m_cfg_file = Ini.IniComponent.Instance.OpenFile(GetPath());
		}

		public T Read<T>(string key, string section = "general")
			=> Ini.IniComponent.Instance.GetBySectionValue<T>(GetNativeIni(), section, key);
	}
}
