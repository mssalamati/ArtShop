using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtShop.Models
{
    public class SearchCollectionViewModel
    {
        public int collectionId { get; set; }
        public int productId { get; set; }
        public string collectionName { get; set; }
    }
}