using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enitities
{
    public class UserProfile
    {
        [Key, ForeignKey("ApplicationUserDetail")]
        public virtual String Id { get; set; }
        public virtual ApplicationUser ApplicationUserDetail { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual DateTime RegisterDate { get; set; }

        public UserProfile()
        {
            RegisterDate = DateTime.Now;
        }
    }
}
