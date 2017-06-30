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
        public virtual int Twitter { get; set; }
        public virtual int Pinterest { get; set; }
        public virtual int Tumblr { get; set; }
        public virtual int Instagram { get; set; }
        public virtual int GooglePlus { get; set; }
        public virtual int Website { get; set; }
    }
}
