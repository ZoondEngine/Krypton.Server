using System.Linq;
using System.Management;
using System.Net.NetworkInformation;

namespace Krypton.MinimalLoader.Core.Hardware.Subtypes
{
	public class NetworkCaption : BaseCaption, IHardwareCaption
	{
		public NetworkCaption() : base(new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapterConfiguration"))
		{ }

		public string GetIpGateway()
			=> Get<string>("DefaultIPGateway");

		public string GetIpAddress()
			=> Get<string>("IPAddress");

		public string GetIpSubnet()
			=> Get<string>("IPSubnet");

		public string GetMacAddress()
		{
			string macAddresses = string.Empty;

			foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
			{
				if (nic.OperationalStatus == OperationalStatus.Up)
				{
					macAddresses += nic.GetPhysicalAddress().ToString();
					break;
				}
			}

			return macAddresses;
		}

		public string GetServiceName()
			=> Get<string>("ServiceName");
	}
}
