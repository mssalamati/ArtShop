using Blog.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class NavigationCategory
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("category")]
        public int categoryId { get; set; }
        public virtual Category category { get; set; }
        public int priority { get; set; }
    }
}