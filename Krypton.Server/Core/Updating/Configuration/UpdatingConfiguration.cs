using Krypton.Server.Core.Ini.Contracts;
using Krypton.Server.Core.Ini.Details;
using System.IO;

namespace Krypton.Server.Core.Updating.Configuration
{
	public class UpdatingConfiguration : IModuleConfiguration
	{
		private IniFile m_ini_file;
		private string m_file_path;

		public T As<T>() where T : IModuleConfiguration
			=> (T)(IModuleConfiguration)this;

		public void Default()
		{
			var path = Path.Combine(GlobalSettings.Configuration.ConfigurationDirectory, GlobalSettings.Configuration.UpdatingComfigurationFile);
			var comp = Ini.IniComponent.Instance;
			var ini_file = comp.CreateIni();

			ini_file["loader"]["version"] = "1.0.483.1";
			ini_file["loader"]["link"] = "http://cdn.kryptonware.xyz/loader/update.exe";
			ini_file["loader"]["link_user"] = "cdn_remote";
			ini_file["loader"]["link_password"] = "PzRcj4Wk3PqQxBm2";

			ini_file["dll"]["use_manual_loading"] = false;
			ini_file["dll"]["allowed_regions"] = "ru,en,bs,fr,tr,es,sk";
			ini_file["dll"]["decline_downloading"] = true;
			ini_file["dll"]["version"] = "7.0.6.3";
			ini_file["dll"]["link"] = "http://control.kryptonware.xyz/storage/storage/app/updated/loader.exe";
			ini_file["dll"]["link_user"] = "cdn_remote_allowed";
			ini_file["dll"]["link_password"] = "FB25vAHZSTR2Osle";

			ini_file.Save(path);
		}

		public string GetName()
			=> "updating-native-configuration";

		public IniFile GetNativeIni()
			=> m_ini_file;

		public string GetPath()
			=> m_file_path;

		public void Load()
		{
			m_file_path = Path.Combine(GlobalSettings.Configuration.ConfigurationDirectory, GlobalSettings.Configuration.UpdatingComfigurationFile);
			if (!File.Exists(GetPath()))
			{
				Default();
			}

			m_ini_file = Ini.IniComponent.Instance.OpenFile(GetPath());
		}

		public T Read<T>(string key, string section) 
			=> Ini.IniComponent.Instance.GetBySectionValue<T>(GetNativeIni(), section, key);
	}
}
