﻿using Krypton.Server.Core.MMF.Details;
using Krypton.Server.Core.Updating.Configuration;
using Krypton.Support;

namespace Krypton.Server.Core.Updating
{
	public class UpdatingComponent : KryptonComponent<UpdatingComponent>
	{
		private MappedFile m_mapped_dll { get; set; }
		private UpdatingConfiguration m_config { get; set; }

		public bool IsDeclineDownloadHack { get; set; }

		public UpdatingComponent()
		{
			m_config = Ini.IniComponent.Instance.GetByName("updating-native-configuration").As<UpdatingConfiguration>();
			IsDeclineDownloadHack = m_config.Read<bool>("decline_downloading", "dll");
		}

		public void ReloadSettings(bool save)
		{
			if(save)
			{
				GetConfig().GetNativeIni().Save(GetConfig().GetPath());
			}

			m_config = null;
			m_config = Ini.IniComponent.Instance.GetByName("updating-native-configuration").As<UpdatingConfiguration>();

			IsDeclineDownloadHack = m_config.Read<bool>("decline_downloading", "dll");
		}

		public UpdatingConfiguration GetConfig()
			=> m_config;
	}
}
