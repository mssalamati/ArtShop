using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enitities
{
    public class PersonalInformation
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual string AboutMe { get; set; }
        public virtual string Education { get; set; }
        public virtual string Events { get; set; }
        public virtual string Exhibitions { get; set; }
    }
}
