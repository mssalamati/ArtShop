using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArtShop.Models
{
    public class ProfileInformationViewModel
    {
        [Display(Name = "Facebook")]
        public string Facebook { get; set; }
        [Display(Name = "Twitter")]
        public string Twitter { get; set; }
        [Display(Name = "Pinterest")]
        public string Pinterest { get; set; }
        [Display(Name = "Tumblr")]
        public string Tumblr { get; set; }
        [Display(Name = "Instagram")]
        public string Instagram { get; set; }
        [Display(Name = "Google Plus")]
        public string GooglePlus { get; set; }
        [Display(Name = "My Website")]
        public string Website { get; set; }
        [Display(Name = "About Me")]
        public string AboutMe { get; set; }
        [Display(Name = "Education")]
        public string Education { get; set; }
        [Display(Name = "Events")]
        public string Events { get; set; }
        [Display(Name = "Exhibitions")]
        public string Exhibitions { get; set; }
        [Display(Name = "Country")]
        public virtual Country country { get; set; }
        public virtual int countryId { get; set; }
        [Display(Name = "City")]
        public virtual string City { get; set; }
        [Display(Name = "Region")]
        public virtual string Region { get; set; }
        [Display(Name = "ZipCode")]
        public virtual string ZipCode { get; set; }
    }
}