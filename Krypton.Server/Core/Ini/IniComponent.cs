using Krypton.Support;
using Krypton.Server.Core.Ini.Details;
using System.IO;
using Krypton.Server.Core.Ini.Contracts;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

namespace Krypton.Server.Core.Ini
{
	public class IniComponent : KryptonComponent<IniComponent>
	{
		private List<IModuleConfiguration> m_configuration_list { get; set; }

		public IniComponent()
		{
			if (!Directory.Exists(GlobalSettings.Configuration.ConfigurationDirectory))
				Directory.CreateDirectory(GlobalSettings.Configuration.ConfigurationDirectory);

			m_configuration_list = LoadConfiguration();
		}

		public void Load()
		{
			foreach (var cfg in m_configuration_list)
			{
				cfg.Load();
			}
		}

		public IniFile GetNativeByName(string name)
			=> GetByName(name)?.GetNativeIni();

		public IModuleConfiguration GetByName(string name)
			=> m_configuration_list.FirstOrDefault((x) => x.GetName().ToLower() == name.ToLower());

		public IniFile OpenFile(string path, bool for_creating = false)
		{
			if (!File.Exists(path))
			{
				if (for_creating)
				{
					return new IniFile();
				}
				else
				{
					return null;
				}
			}
			else
			{
				var ini = new IniFile();
				ini.Load(path);

				return ini;
			}
		}

		public IniFile CreateIni()
		{
			return new IniFile();
		}

		public T GetBySectionValue<T>(IniFile file, string section, string variable)
		{
			if(file == null)
			{
				return default;
			}

			if (file.ContainsSection(section))
			{
				var readed_section = file[section];
				if (readed_section.ContainsKey(variable))
				{
					return readed_section[variable].Get<T>();
				}
			}

			return default;
		}

		private List<IModuleConfiguration> LoadConfiguration()
		{
			return Assembly
				.GetExecutingAssembly()
				.GetTypes()
				.Where(m => m.GetInterfaces().Contains(typeof(IModuleConfiguration)))
				.Select(m => m.GetConstructor(Type.EmptyTypes).Invoke(null) as IModuleConfiguration)
				.ToList();
		}
	}
}
