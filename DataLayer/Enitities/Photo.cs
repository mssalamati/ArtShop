using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enitities
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }
        public string Path { get; set; }
        public float width { get; set; }
        public float Height { get; set; }
    }
}
