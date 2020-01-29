using System.Linq;

namespace Krypton.Server.Core.Journaling.Types
{
	public class DriveStamp
	{
		public string Interface { get; private set; }
		public string Manufacturer { get; private set; }
		public string Model { get; private set; }
		public string Serial { get; private set; }

		public DriveStamp(string[] raw)
		{
			if(raw.Count() == 4)
			{
				Interface    = raw[0];
				Manufacturer = raw[1];
				Model        = raw[2];
				Serial       = raw[3];
			}
		}
	}
}
