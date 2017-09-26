using DataLayer.Enitities;

namespace ArtShop.Controllers
{
    public class productemailviewmodel
    {

        public productemailviewmodel()
        {
        }

        public string photo { get; set; }
        public decimal quantity { get; set; }
        public string title { get; set; }
        public decimal unitPrice { get; set; }
        public Productpackage package { get; set; }
    }
}