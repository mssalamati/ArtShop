﻿using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtShop.Models
{
    public class CollectionViewModel
    {
        public int CollectionId { get; set; }
        public string CollectionName { get; set; }
        public ICollection<CollectionProduct> collectionProduct { get; set; }

    }
}