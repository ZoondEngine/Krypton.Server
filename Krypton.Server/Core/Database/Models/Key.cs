using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Krypton.Server.Core.Database.Models
{
    [Table("keys")]
    public class Key
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("value")]
        public string Value { get; set; }

        [Column("days")]
        public int Days { get; set; }

        [Column("region")]
        public int RegionCode { get; set; }

        [Column("packId")]
        public int PacketId { get; set; }

        [Column("creatorId")]
        public int OwnerId { get; set; }

        [Column("enabled")]
        public int Enabled { get; set; }

        [Column("blocked")]
        public int Blocked { get; set; }

        [Column("hardware")]
        public string Hardware { get; set; }

        [Column("activate_date")]
        public DateTime ActivatedAt { get; set; }

        [Column("end_date")]
        public DateTime EndAt { get; set; }

        public bool IsBlocked()
            => Blocked == 1;

        public bool IsEnabled()
            => Enabled == 1;
    }
}
