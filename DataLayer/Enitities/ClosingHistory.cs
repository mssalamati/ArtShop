using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace DataLayer.Enitities
{
    public class ClosingHistory
    {
        public int ClosingHistoryID { get; set; }
        [ForeignKey("Bid")]
        public int BidID { get; set; }
        [ForeignKey("Listing")]
        public int ListingID { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }
        public virtual Bid Bid { get; set; }
        public virtual Listing Listing { get; set; }
        public virtual UserProfile User { get; set; }
    }
}