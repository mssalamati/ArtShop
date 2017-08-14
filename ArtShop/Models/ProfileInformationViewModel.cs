using DataLayer.Enitities;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArtShop.Models
{
    public class ProfileInformationViewModel
    {
        [Display(Name = nameof(ProfileRes.Facebook), ResourceType = typeof(ProfileRes))]
        public string Facebook { get; set; }
        [Display(Name = nameof(ProfileRes.Twitter), ResourceType = typeof(ProfileRes))]
        public string Twitter { get; set; }
        [Display(Name = nameof(ProfileRes.Pinterest), ResourceType = typeof(ProfileRes))]
        public string Pinterest { get; set; }
        [Display(Name = nameof(ProfileRes.Tumblr), ResourceType = typeof(ProfileRes))]
        public string Tumblr { get; set; }
        [Display(Name = nameof(ProfileRes.Instagram), ResourceType = typeof(ProfileRes))]
        public string Instagram { get; set; }
        [Display(Name = nameof(ProfileRes.Google_plus), ResourceType = typeof(ProfileRes))]
        public string GooglePlus { get; set; }
        [Display(Name = nameof(ProfileRes.My_website), ResourceType = typeof(ProfileRes))]
        public string Website { get; set; }
        [Display(Name = nameof(ProfileRes.About_me), ResourceType = typeof(ProfileRes))]
        public string AboutMe { get; set; }
        [Display(Name = nameof(ProfileRes.Education), ResourceType = typeof(ProfileRes))]
        public string Education { get; set; }
        [Display(Name = nameof(ProfileRes.Events), ResourceType = typeof(ProfileRes))]
        public string Events { get; set; }
        [Display(Name = nameof(ProfileRes.Exhibitions), ResourceType = typeof(ProfileRes))]
        public string Exhibitions { get; set; }
        [Display(Name = nameof(ProfileRes.Country), ResourceType = typeof(ProfileRes))]
        public virtual Country country { get; set; }
        public virtual int countryId { get; set; }
        [Display(Name = nameof(ProfileRes.City), ResourceType = typeof(ProfileRes))]
        public virtual string City { get; set; }
        [Display(Name = nameof(ProfileRes.Region), ResourceType = typeof(ProfileRes))]
        public virtual string Region { get; set; }
        [Display(Name = nameof(ProfileRes.ZipCode), ResourceType = typeof(ProfileRes))]
        public virtual string ZipCode { get; set; }
    }
}