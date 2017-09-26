using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileApi.Models
{
    public class UploadViewModel
    {
        public string LanguageCode { get; set; }
        public int SubjectId { get; set; }
        public int categoryId { get; set; }
        public int createDate { get; set; }
        public bool isforsale { get; set; }
        public string mediums { get; set; }
        public string materials { get; set; }
        public string styles { get; set; }
        public string keywords { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
        public float Depth { get; set; }
        public float weight { get; set; }
        public string Title { get; set; }
        public int Limitform { get; set; }
        public int LimitOf { get; set; }
        public string Description { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public int Country { get; set; }
        public string Region { get; set; }
        public string Zipcode { get; set; }
        public string Phonenumber { get; set; }
        public decimal Price { get; set; }    
        public sizeMV SqrResizeRect { get; set; }
        public sizeMV WideResizeRect { get; set; }
        public HttpPostedFileBase Image { get; set; }
    }

    public class sizeMV
    {
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }

    }
}