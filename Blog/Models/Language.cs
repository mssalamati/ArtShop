using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Objects
{
    public class Language
    {
        [Key]
        [MaxLength(10)]
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
