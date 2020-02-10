using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.Server.Core.Database.Models
{
	[Table("keys")]
	public class Update
	{
		[Column("id")]
		public long Id { get; set; }
	}
}
