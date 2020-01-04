using System.Management;

namespace Krypton.MinimalLoader.Core.Hardware.Subtypes
{
	public class DiskDriveCaption : BaseCaption, IHardwareCaption
	{
		public DiskDriveCaption() : base(new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive"))
		{ }

		public string GetInterfaceType()
			=> Get<string>("InterfaceType");

		public string GetManufacturer()
			=> Get<string>("Manufacturer");

		public string GetModel()
			=> Get<string>("Model");

		public string GetSerialNumber()
			=> Get<string>("SerialNumber");
	}
}
