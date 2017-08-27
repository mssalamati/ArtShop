using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Objects
{
    public class HeaderPhoto
    {
        [Key]
        public int Id { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
    }
}