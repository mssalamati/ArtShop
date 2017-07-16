using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enitities
{
    public class UserLink
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual string Facebook { get; set; }
        public virtual string Twitter { get; set; }
        public virtual string Pinterest { get; set; }
        public virtual string Tumblr { get; set; }
        public virtual string Instagram { get; set; }
        public virtual string GooglePlus { get; set; }
        public virtual string Website { get; set; }
    }
}
