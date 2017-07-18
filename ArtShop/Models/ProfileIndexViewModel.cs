using DataLayer.Enitities;
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
        public Country country { get; set; }
        public string city { get; set; }
        public string region { get; set; }
        public List<Product> artworks { get; set; }

    }
}