using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtShop.Models
{
    public class ShoppingCartViewModel
    {
        public List<ShoppingCart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }

    public class ShoppingCartRemoveViewModel
    {
        public string Message { get; set; }
        public decimal CartTotal { get; set; }
        public int CartCount { get; set; }
        public decimal ItemCount { get; set; }
        public int DeleteId { get; set; }
    }
}