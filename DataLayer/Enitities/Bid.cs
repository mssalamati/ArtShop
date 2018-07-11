using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Enitities
{
    public class Bid
    {
        public int BidID { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }
        [ForeignKey("Listing")]
        public int ListingID { get; set; }
        public int? CurrentPrice { get; set; }
        public DateTime Timestamp { get; set; }
        public int Amount { get; set; }
        public virtual UserProfile User { get; set; }
        public virtual Listing Listing { get; set; }

    }
}