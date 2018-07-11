
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Enitities
{
    public class Watch
    {
        public int WatchID { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }
        [ForeignKey("Listing")]
        public int ListingID { get; set; }
        public virtual UserProfile User { get; set; }
        public virtual Listing Listing { get; set; }
    }
}