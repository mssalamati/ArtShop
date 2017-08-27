using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Objects
{
    public class Link
    {
        public virtual int Id { get; set; }
        public virtual string URL { get; set; }
    }
}