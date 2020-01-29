using System.Linq;

namespace Krypton.Server.Core.Journaling.Types
{
	public class NetworkStamp
	{
		public string IpGateway { get; private set; }
		public string IpAddress { get; private set; }
		public string IpSubnet { get; private set; }
		public string MacAddress { get; private set; }
		public string ServiceName { get; private set; }

		public NetworkStamp(string[] raw)
		{
			if(raw.Count() == 5)
			{
				IpGateway   = raw[0];
				IpAddress   = raw[1];
				IpSubnet    = raw[2];
				MacAddress  = raw[3];
				ServiceName = raw[4];
			}
		}
	}
}
