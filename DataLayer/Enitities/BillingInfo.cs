using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enitities
{
    public class BillingInfo
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Unit { get; set; }
        [ForeignKey("country")]
        public int CountryId { get; set; }
        public virtual Country country { get; set; }
    }
}
