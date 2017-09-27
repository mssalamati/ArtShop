using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileApi.Models
{
    public class ResizeViewModel
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
}