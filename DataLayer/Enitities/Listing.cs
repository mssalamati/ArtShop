using DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Enitities
{
    public class Listing: ITranslatable<Listing, ListingTranslation>
    {
        public int ListingID { get; set; }
        public int auctionInfoId { get; set; }
        public virtual ICollection<ListingTranslation> Translations { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime EndTimestamp { get; set; }
        public string ImageUrl { get; set; }
        public int StartingPrice { get; set; }
        public bool Active { get; set; }
        public bool ShowWinner { get; set; }
        public int BidStep { get; set; }
        [ForeignKey("Winner")]
        public string WinnerId { get; set; }
        public virtual UserProfile Winner { get; set; }
        public virtual ICollection<Watch> Watches { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }      
        public virtual Product Artwork { get; set; }
        [NotMapped]
        public virtual int ArtworkId { get; set; }
        [NotMapped]
        public virtual string StartTime { get; set; }
        [NotMapped]
        public virtual string EndTime { get; set; }
    }
    public class ListingTranslation : ITranslation<Listing>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [ForeignKey("listing")]
        [Key, Column(Order = 1)]
        public virtual int listingID { get; set; }
        public virtual Listing listing { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}