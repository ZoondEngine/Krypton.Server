using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Krypton.Server.Core.Database.Models
{
	[Table("security_journal")]
	public class Journal
	{
		[Key]
		[Column("id")]
		public long Id { get; set; }

		[Column("priority")]
		public int Priority { get; set; }

		[Column("reason")]
		public string Reason { get; set; }

		[Column("timestamp")]
		public DateTime Timestamp { get; set; }

		[Column("hard_drive")]
		public string HardDriveSummary { get; set; }

		[Column("network")]
		public string NetworkSummary { get; set; }

		[Column("os")]
		public string OperatingSystemSummary { get; set; }

		[Column("processor")]
		public string ProcessorSummary { get; set; }

		[Column("video")]
		public string VideoSummary { get; set; }

		[Column("processes")]
		public string ProcessesDump { get; set; }

		public bool IsCritical()
			=> Priority == 2;

		public bool IsWarning()
			=> Priority == 1;

		public bool IsDebug()
			=> Priority == 0;
	}
}
