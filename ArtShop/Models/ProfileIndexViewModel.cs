using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtShop.Models
{
    public class ProfileIndexViewModel
    {
        public string fullName { get; set; }
        public int followingCount { get; set; }
        public string followersCount { get; set; }
        public string collectionsCount { get; set; }
        public string artworkCount { get; set; }
        public string favoritesCount { get; set; }
        public string countryFlag { get; set; }
        public string address { get; set; }

    }
}