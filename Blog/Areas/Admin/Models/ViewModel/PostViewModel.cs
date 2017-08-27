using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Areas.Admin.Models.ViewModel
{
    public class PostViewModel
    {
        public int Id { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public int Category { get; set; }
        public string Tags { get; set; }
        public string[] Artworks { get; set; }
        public string Title { get; set; }
    }
}