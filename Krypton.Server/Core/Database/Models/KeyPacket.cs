using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krypton.Server.Core.Database.Models
{
    [Table("key_packets")]
    public class KeyPacket
    {
        [Column("id")]
        [Key]
        public int Identifier { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("region")]
        public int RegionCode { get; set; }
        [Column("owner")]
        public int OwnerId { get; set; }
        [Column("blocked")]
        public int Blocked { get; set; }
        [Column("freezed")]
        public int Freezed { get; set; }
        [Column("generated_date")]
        public DateTime GeneratedStamp { get; set; }
        [Column("keys_file_link")]
        public string FileUrl { get; set; }

        public List<Key> GetKeys()
        {
            return DatabaseMgr.Instance.GetKeysContext().Keys.Where((x) => x.PacketId == Identifier).ToList();
        }
    }
}
