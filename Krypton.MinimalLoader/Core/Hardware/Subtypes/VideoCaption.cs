using System.Management;

namespace Krypton.MinimalLoader.Core.Hardware.Subtypes
{
	public class VideoCaption : BaseCaption, IHardwareCaption
	{
		public VideoCaption() : base(new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController"))
		{ }

		public string GetRam()
			=> Get<string>("AdapterRAM");

		public string GetVideoProcessor()
			=> Get<string>("VideoProcessor");
	}
}
