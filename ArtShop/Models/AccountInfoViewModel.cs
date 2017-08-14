using DataLayer.Enitities;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArtShop.Models
{
    public class AccountInfoViewModel
    {
        [Required]
        [Display(Name = nameof(ProfileRes.First_Name), ResourceType = typeof(ProfileRes))]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = nameof(ProfileRes.Last_name), ResourceType = typeof(ProfileRes))]
        public string LastName { get; set; }
        [Required]
        [Display(Name = nameof(ProfileRes.Email_address), ResourceType = typeof(ProfileRes))]
        public string Email { get; set; }
        [Display(Name = nameof(ProfileRes.New_password), ResourceType = typeof(ProfileRes))]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = nameof(ProfileRes.Confirm_password), ResourceType = typeof(ProfileRes))]
        public string ConfirmPassword { get; set; }
        [Display(Name = nameof(ProfileRes.Mailing_list), ResourceType = typeof(ProfileRes))]
        public bool MailingList { get; set; }
        [Display(Name = nameof(ProfileRes.Receive_Art_Uploaded_Email), ResourceType = typeof(ProfileRes))]
        public bool ReceiveNewArtEmail { get; set; }
        [Display(Name = nameof(ProfileRes.Profile_type), ResourceType = typeof(ProfileRes))]
        public ProfileType profileType { get; set; }
    }
}