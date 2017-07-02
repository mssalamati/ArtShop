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
        public int followersCount { get; set; }
        public int collectionsCount { get; set; }
        public int artworkCount { get; set; }
        public int favoritesCount { get; set; }
        public string countryFlag { get; set; }
        public string address { get; set; }

    }
}