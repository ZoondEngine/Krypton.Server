using Krypton.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using Krypton.MinimalLoader.Core.Hardware.Subtypes;
using System.Reflection;
using System.Diagnostics;

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

		public string BuildLog()
		{
			string log = "";

			var network = GetCaption<NetworkCaption>();
			var disk = GetCaption<DiskDriveCaption>();
			var processor = GetCaption<ProcessorCaption>();
			var os = GetCaption<OSCaption>();
			var video = GetCaption<VideoCaption>();

			/* Building network */
			log += network.GetMacAddress().Trim() + "_NETWORK;";
			log += network.GetServiceName().Trim() + "_NETWORK;";

			/* Building hard drive */
			log += disk.GetInterfaceType().Trim() + "_HDD;";
			log += disk.GetManufacturer().Trim() + "_HDD;";
			log += disk.GetModel().Trim() + "_HDD;";
			log += disk.GetSerialNumber().Trim() + "_HDD;";

			/* Building processor */
			log += processor.GetProcessorId().Trim() + "_PROCESSOR;";
			log += processor.GetProcessorName().Trim() + "_PROCESSOR;";

			/* Building os */
			log += os.GetFreePhysicalMemory().Trim() + "_OpSy;";
			log += os.GetSerialNumber().Trim() + "_OpSy;";
			log += os.GetVersion().Trim() + "_OpSy;";

			/* Building processes list */
			var processes = Process.GetProcesses();
			foreach(var process in processes)
			{
				try
				{
					log += $"{process.ProcessName} ID-{process.Id} HANDLE-0x{process.Handle.ToString("X4")}_PROCESS;";

					//Very much  more infos
					//log += $"{process.ProcessName} modules: \n";
					//foreach (ProcessModule module in process.Modules)
					//{
					//	log += $" -- MODULE-{module.ModuleName}+FILE-{module.FileName}+BASEADDRESS-{module.BaseAddress.ToString("X16")};\n";
					//}
				}
				catch
				{
					continue;
				}
			}

			return log;
		}

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
