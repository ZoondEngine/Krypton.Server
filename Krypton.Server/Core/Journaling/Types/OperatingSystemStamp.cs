using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.Server.Core.Journaling.Types
{
	public class OperatingSystemStamp
	{
		public string FreePhysicalMemory { get; private set; }
		public string MemoryMark { get; private set; }
		public string Name { get; private set; }
		public string Serial { get; private set; }
		public string User { get; private set; }
		public string Version { get; private set; }

		public OperatingSystemStamp(string[] raw)
		{
			if(raw.Count() == 6)
			{
				FreePhysicalMemory = raw[0];
				MemoryMark = raw[1];
				Name = raw[2];
				Serial = raw[3];
				User = raw[4];
				Version = raw[5];
			}
		}
	}
}
