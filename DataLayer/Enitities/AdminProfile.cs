using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enitities
{
    public class AdminProfile
    {
        [Key, ForeignKey("ApplicationAdminDetail")]
        public virtual String Id { get; set; }
        public virtual ApplicationUser ApplicationAdminDetail { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }
}
