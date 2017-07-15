using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enitities
{
    public enum ProfileType{Artist,Collector}
    public class UserProfile
    {
        [Key, ForeignKey("ApplicationUserDetail")]
        public virtual String Id { get; set; }
        public virtual ApplicationUser ApplicationUserDetail { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        [ForeignKey("country")]
        public int? countryId { get; set; }
        public virtual Country country { get; set; }
        public virtual string City { get; set; }
        public virtual string Region { get; set; }
        public virtual string ZipCode { get; set; }
        public bool MailingList { get; set; }
        public bool ReceiveNewArtEmail { get; set; }
        public virtual PersonalInformation personalInformation { get; set; }
        public virtual UserLink userLinks { get; set; }
        public virtual ProfileType profileType { get; set; }
        public virtual DateTime RegisterDate { get; set; }
        public virtual ICollection<Collection> Collections { get; set; }
        public virtual ICollection<Product> Products { get; set; }

        public UserProfile()
        {
            RegisterDate = DateTime.Now;
        }
    }
}
