using Krypton.Server.Core.Ini.Details;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.Server.Core.Ini.Contracts
{
	public interface IModuleConfiguration
	{
		string GetName();
		string GetPath();
		IniFile GetNativeIni();
		T Read<T>(string key, string section);
		void Default();
		void Load();
		T As<T>() where T : IModuleConfiguration;
	}
}
