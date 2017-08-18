using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtShop.Models
{
    public class ProfileIndexViewModel
    {
        public string id { get; set; }
        public string fullName { get; set; }
        public int followingCount { get; set; }
        public int followersCount { get; set; }
        public int collectionsCount { get; set; }
        public int artworkCount { get; set; }
        public int favoritesCount { get; set; }
        public Country country { get; set; }
        public string city { get; set; }
        public string photoPath { get; set; }
        public string region { get; set; }
        public string facebook { get; set; }
        public string twitter { get; set; }
        public string pinterest { get; set; }
        public string tumbler { get; set; }
        public string instagram { get; set; }
        public string googlePlus { get; set; }
        public string myWebsite { get; set; }
        public string aboutme { get; set; }
        public string education { get; set; }
        public string events { get; set; }
        public string Exhibitions { get; set; }
        public List<Product> artworks { get; set; }

    }
}