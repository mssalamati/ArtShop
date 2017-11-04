using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArtShop.Models
{
    public class UploadViewModel
    {
        public class step1
        {
            public string img { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }
        public class step2
        {
            public int subject { get; set; }
            public int category { get; set; }
        }
        public class step3
        {
            public int createYear { get; set; }
            public bool isOrginal { get; set; }
            public bool printAvable { get; set; }
            public bool copyright { get; set; }
        }
        public class step4
        {
            public string image { get; set; }
            public float square_x { get; set; }
            public float square_y { get; set; }
            public float square_width { get; set; }
            public float square_height { get; set; }
            public float wide_x { get; set; }
            public float wide_y { get; set; }
            public float wide_width { get; set; }
            public float wide_height { get; set; }
        }
        public class step5
        {
            public string Mediums { get; set; }
            public int[] Materials { get; set; }
            public string Styles { get; set; }
            public string Keywords { get; set; }
        }
        public class step6
        {
            public float Height { get; set; }
            public float Width { get; set; }
            public float Depth { get; set; }
        }
        public class step7
        {
            [StringLength(35, ErrorMessage = "Title cannot be longer than 35 characters.")]
            public string Title { get; set; }
            public int avaible { get; set; }
            public int AllEntity { get; set; }
            public string Description { get; set; }
        }
        public class step8
        {
            public int Packaging { get; set; }
            public bool framed { get; set; }
            public bool multi_paneled { get; set; }
            public string type { get; set; }
            public string matter { get; set; }
            public string color { get; set; }
        }
        public class step9
        {
            public float weight { get; set; }

            public string Street_Address { get; set; }
            public string Address_2 { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public string Region { get; set; }
            public string Zip_code { get; set; }
            public string phoneNumber { get; set; }
        }
        public class step10
        {
            public float Price { get; set; }
        }

        public class step9_5
        {
            public int[] printtypes { get; set; }
        }
    }
}