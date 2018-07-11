using DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Enitities
{
    public class AuctionInfo: ITranslatable<AuctionInfo, AuctionInfoTranslation>
    {
        public int Id { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime EndTimestamp { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<AuctionInfoTranslation> Translations { get; set; }        
        public virtual ICollection<Listing> Listings { get; set; }
        [NotMapped]
        public virtual string StartTime { get; set; }
        [NotMapped]
        public virtual string EndTime { get; set; }
    }
    public class AuctionInfoTranslation : ITranslation<AuctionInfo>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [ForeignKey("auctionInfo")]
        [Key, Column(Order = 1)]
        public virtual int Id { get; set; }
        public virtual AuctionInfo auctionInfo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}