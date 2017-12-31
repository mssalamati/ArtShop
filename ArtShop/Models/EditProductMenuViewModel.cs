using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtShop.Models
{
    public class EditProductMenuViewModel
    {   
        public string Photo { get; set; }
        public int id { get; set; }
        public string menu { get; set; }
        public ProductStatus Status { get; set; }
    }
}