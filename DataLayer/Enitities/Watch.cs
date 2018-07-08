
namespace DataLayer.Enitities
{
    public class Watch
    {
        public int WatchID { get; set; }
        public int UserID { get; set; }
        public int ListingID { get; set; }
        public virtual UserProfile User { get; set; }
        public virtual Listing Listing { get; set; }
    }
}