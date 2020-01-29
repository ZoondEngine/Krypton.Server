using Krypton.Server.Core.Journaling.Types;
using Krypton.Support;
using System.Linq;

namespace Krypton.Server.Core.Journaling
{
	public class Journal
	{
		private DriveStamp HardDrive { get; set; }
		private NetworkStamp Network { get; set; }
		private OperatingSystemStamp OperatingSystem { get; set; }
		private ProcessorStamp Processor { get; set; }
		private VideoStamp VideoAdapter { get; set; }

		public void Load(string[] parsable_data)
		{
			var list_data = parsable_data.ToList();
			HardDrive = new DriveStamp
				(
				list_data.Where((x) => x.ToLower().Contains("_HDD"))
					.Select((x) => x.Replace("_HDD", ""))
					.ToArray()
				);

			Network = new NetworkStamp
				(
				list_data.Where((x) => x.ToLower().Contains("_NETWORK"))
					.Select((x) => x.Replace("_NETWORK", ""))
					.ToArray()
				);

			OperatingSystem = new OperatingSystemStamp
				(
				list_data.Where((x) => x.ToLower().Contains("_OpSy"))
					.Select((x) => x.Replace("_OpSy", ""))
					.ToArray()
				);

			Processor = new ProcessorStamp
				(
				list_data.Where((x) => x.ToLower().Contains("_PROCESSOR"))
					.Select((x) => x.Replace("_PROCESSOR", ""))
					.ToArray()
				);

			VideoAdapter = new VideoStamp
				(
				list_data.Where((x) => x.ToLower().Contains("_VIDEO"))
					.Select((x) => x.Replace("_VIDEO", ""))
					.ToArray()
				);
		}

		public void ToDatabase()
		{

		}

		public void ToDirectoryLog()
		{

		}

		public DriveStamp GetDrive()
			=> HardDrive;

		public NetworkStamp GetNetwork()
			=> Network;

		public OperatingSystemStamp GetOS()
			=> OperatingSystem;

		public ProcessorStamp GetProcessor()
			=> Processor;

		public VideoStamp GetVideo()
			=> VideoAdapter;
	}
}
