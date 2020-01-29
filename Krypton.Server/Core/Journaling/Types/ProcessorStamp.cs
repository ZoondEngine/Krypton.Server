using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.Server.Core.Journaling.Types
{
	public class ProcessorStamp
	{
		public string Identifier { get; private set; }
		public string Name { get; private set; }

		public ProcessorStamp(string[] raw)
		{
			if(raw.Count() == 2)
			{
				Identifier = raw[0];
				Name = raw[1];
			}
		}
	}
}
