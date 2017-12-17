using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Enitities
{
    public class FAQRequest
    {
        public int Id { get; set; }
        public string type { get; set; }
        [Required]
        //[DisplayName(Resources.Resources.labelForName)]
        [EmailAddress]
        public string email { get; set; }
        public string subject { get; set; }
        public string description { get; set; }
        public string question { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string AttachmentAddress { get; set; }
        public string ArtistName { get; set; }
        public string ArtworkTitle { get; set; }
        [NotMapped]
        public HttpPostedFileBase Attachments { get; set; }
    }
}
