using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArtShop.Models
{
    public class VisitRequestViewModel
    {
        [Required]
        [Display(Name = nameof(ProfileRes.First_Name), ResourceType = typeof(ProfileRes))]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = nameof(ProfileRes.Last_name), ResourceType = typeof(ProfileRes))]
        public string LastName { get; set; }
        [Required]
        [Display(Name = nameof(ProfileRes.PhoneNumber), ResourceType = typeof(ProfileRes))]
        public string PhoneNumber { get; set; }
        [Display(Name = nameof(UploadRes.description), ResourceType = typeof(UploadRes))]
        public string Description { get; set; }
        public int Id { get; set; }
    }
}