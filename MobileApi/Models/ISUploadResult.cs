using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileApi.Models
{
    public class ISUploadResult
    {
        public bool result { get; set; }
        public string data { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}