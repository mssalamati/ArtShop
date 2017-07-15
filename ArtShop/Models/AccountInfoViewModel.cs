using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArtShop.Models
{
    public class AccountInfoViewModel
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        [Display(Name = "New Password")]
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Mailing List")]
        public bool MailingList { get; set; }
        [Display(Name = "Receive Art Uploaded Email")]
        public bool ReceiveNewArtEmail { get; set; }
        [Display(Name = "Profile Type")]
        public ProfileType profileType { get; set; }
    }
}