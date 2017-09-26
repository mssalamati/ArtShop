using DataLayer.Enitities;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArtShop.Models
{
    public class NewCollectionViewModel
    {
        public int CollectionId { get; set; }
        
        [Display(Name = nameof(ProfileRes.Collection_title), ResourceType = typeof(ProfileRes))]
        public string CollectionTitle { get; set; }

        [Display(Name = nameof(ProfileRes.Collection_description), ResourceType = typeof(ProfileRes))]
        public string CollectionDescription { get; set; }
        public ICollection<CollectionProduct> collectionProduct { get; set; }
        public CollectionType CollectionType { get; set; }
        [Display(Name = nameof(ProfileRes.Private), ResourceType = typeof(ProfileRes))]
        public bool IsPrivate { get; set; }

    }
}