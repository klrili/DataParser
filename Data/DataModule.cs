using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SteamBigData.Data
{
    public class SoldInfo
    {
        public int id { get; set; }

        public int itemNameId { get; set; }

        [MaxLength(50)]

        public string? buyerUserName { get; set; }

        [MaxLength(100)]

        public string? buyerAvatarUrl { get; set; }

        [MaxLength(50)]

        public string? sellerUserName { get; set; }

        [MaxLength(100)]

        public string? sellerAvatarUrl { get; set; }

        [Column(TypeName = "decimal(6 ,2)")]

        public decimal price { get; set; }

        public int timestamp { get; set; }
    }
}

