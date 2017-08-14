using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtShop.Models
{
    public class HomeIndexViewModel
    {
        public string slider_H1 { get; set; }
        public string slider_H2 { get; set; }
        public string slider_Button_Text { get; set; }
        public string slider_P { get; set; }
        public string slider_Button_Url { get; set; }
        public string slider_Button_color { get; set; }
        public string Slider_Image { get; set; }
        public string slider_text_color { get; set; }
        public List<IdNameViewModel> Navigation { get; set; }
    }

    public class IdNameViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public virtual ICollection<IdNameViewModel> FavStyles { get; set; }
        public virtual ICollection<IdNameViewModel> FavMediums { get; set; }
        public virtual ICollection<IdNameViewModel> FavSubjects { get; set; }
    }
}