using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [Display(Name = "FAQ_Type", ResourceType = typeof(ShareRes))]
        [Required]
        public int type { get; set; }

        [Display(Name = "FAQ_email", ResourceType = typeof(ShareRes))]
        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Display(Name = "FAQ_subject", ResourceType = typeof(ShareRes))]
        public string subject { get; set; }

        [Display(Name = "FAQ_description", ResourceType = typeof(ShareRes))]
        public string description { get; set; }

        [Display(Name = "FAQ_question", ResourceType = typeof(ShareRes))]
        [Required]
        public string question { get; set; }

        [Display(Name = "FAQ_Name", ResourceType = typeof(ShareRes))]
        [Required]
        public string Name { get; set; }

        [Display(Name = "FAQ_URL", ResourceType = typeof(ShareRes))]
        public string URL { get; set; }

        [Display(Name = "FAQ_PhoneNumber", ResourceType = typeof(ShareRes))]
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    
        [Display(Name = "FAQ_ArtistName", ResourceType = typeof(ShareRes))]
        public string ArtistName { get; set; }

        [Display(Name = "FAQ_ArtworkTitle", ResourceType = typeof(ShareRes))]
        public string ArtworkTitle { get; set; }

        [NotMapped]
        [Display(Name = "FAQ_Attachment", ResourceType = typeof(ShareRes))]
        public HttpPostedFileBase Attachments { get; set; }
        public string AttachmentAddress { get; set; }

    }
}
