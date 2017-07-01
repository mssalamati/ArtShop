using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArtShop.Models
{
    public class NewCollectionViewModel
    {
        [Display(Name = "Collection Title")]
        public string CollectionTitle { get; set; }

        [Display(Name = "Collection Description")]
        public string CollectionDescription { get; set; }
        public CollectionType CollectionType { get; set; }
        [Display(Name = "Private")]
        public bool IsPrivate { get; set; }

    }
}