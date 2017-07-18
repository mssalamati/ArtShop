using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enitities
{
    public class BillingInfo
    {
        public int Id { get; set; }
        [Display(Name = "Street")]
        public string Street { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Region")]
        public string Region { get; set; }
        [Display(Name = "ZipCode")]
        public string ZipCode { get; set; }
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Unit")]
        public string Unit { get; set; }
        [ForeignKey("country")]
        public int CountryId { get; set; }
        [Display(Name = "Country")]
        public virtual Country country { get; set; }
    }
}
