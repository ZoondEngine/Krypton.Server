using System.Linq;

namespace Krypton.Server.Core.Journaling.Types
{
	public class VideoStamp
	{
		public string AvailableMemory { get; private set; }
		public string ProcessorIdentifier { get; private set; }

		public VideoStamp(string[] raw)
		{
			if(raw.Count() == 2)
			{
				AvailableMemory = raw[0];
				ProcessorIdentifier = raw[1];
			}
		}
	}
}
