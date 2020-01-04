using Krypton.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using Krypton.MinimalLoader.Core.Hardware.Subtypes;
using System.Reflection;

namespace Krypton.MinimalLoader.Core.Hardware
{
	public class HardwareComponent : KryptonComponent<HardwareComponent>
	{
		private LocaleContext Locale;
		private List<IHardwareCaption> Captions;

		public HardwareComponent()
		{
			Locale = new LocaleContext();
			Captions = GetImplementedCaptions();
		}

		public string GetHardwareId()
		{
			return GetCaption<NetworkCaption>().GetMacAddress().Trim()
				+ "__" + GetCaption<DiskDriveCaption>().GetSerialNumber().Trim() 
				+ "__" + GetCaption<ProcessorCaption>().GetProcessorId().Trim();
		}

		public T GetCaption<T>() where T : BaseCaption
		{
			if(Captions.Count > 0)
			{
				return (T)Captions.FirstOrDefault((x) => x.GetType() == typeof(T));
			}

			return (T)(object)null;
		}
		public LocaleContext GetLocale()
			=> Locale;

		private List<IHardwareCaption> GetImplementedCaptions()
		{
			return Assembly
						.GetExecutingAssembly()
						.GetTypes()
						.Where(m => m.GetInterfaces().Contains(typeof(IHardwareCaption)))
						.Select(m => m.GetConstructor(Type.EmptyTypes).Invoke(null) as IHardwareCaption)
						.ToList();
		}
	}
}
