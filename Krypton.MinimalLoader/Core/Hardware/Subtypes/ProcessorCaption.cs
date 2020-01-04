using System.Management;

namespace Krypton.MinimalLoader.Core.Hardware.Subtypes
{
	public class ProcessorCaption : BaseCaption, IHardwareCaption
	{
		public ProcessorCaption() : base(new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor"))
		{ }

		public string GetProcessorId()
			=> Get<string>("ProcessorId");

		public string GetProcessorName()
			=> Get<string>("Name");
	}
}
