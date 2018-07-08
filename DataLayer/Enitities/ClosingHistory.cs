using System;
using System.Web;

namespace DataLayer.Enitities
{
    public class ClosingHistory
    {
        public int ClosingHistoryID { get; set; }
        public int BidID { get; set; }
        public int ListingID { get; set; }
        public int UserID { get; set; }
        public virtual Bid Bid { get; set; }
        public virtual Listing Listing { get; set; }
        public virtual UserProfile User { get; set; }
    }
}