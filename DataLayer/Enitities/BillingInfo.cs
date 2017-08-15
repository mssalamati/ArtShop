using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resources;

namespace DataLayer.Enitities
{
    public class BillingInfo
    {
        public int Id { get; set; }
        [Display(Name = nameof(ProfileRes.Street), ResourceType = typeof(ProfileRes))]
        public string Street { get; set; }
        [Display(Name = nameof(ProfileRes.City), ResourceType = typeof(ProfileRes))]
        public string City { get; set; }
        [Display(Name = nameof(ProfileRes.Region), ResourceType = typeof(ProfileRes))]
        public string Region { get; set; }
        [Display(Name = nameof(ProfileRes.ZipCode), ResourceType = typeof(ProfileRes))]
        public string ZipCode { get; set; }
        [Display(Name = nameof(ProfileRes.PhoneNumber), ResourceType = typeof(ProfileRes))]
        public string PhoneNumber { get; set; }
        [Display(Name = nameof(ProfileRes.Unit), ResourceType = typeof(ProfileRes))]
        public string Unit { get; set; }
        [ForeignKey("country")]
        public int CountryId { get; set; }
        [Display(Name = nameof(ProfileRes.Country), ResourceType = typeof(ProfileRes))]
        public virtual Country country { get; set; }
    }
}
