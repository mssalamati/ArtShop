using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Objects
{
    public class Link
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual string URL { get; set; }
    }
}