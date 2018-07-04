using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enitities
{
    public class VisitRequest
    {
        public int Id { get; set; }
        [ForeignKey("user")]
        public virtual String user_id { get; set; }        
        public virtual UserProfile user { get; set; }
        public int ArtworkID { get; set; }
        public DateTime SubmittedOn { get; set; }
        public string Description { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public bool isConfirmed { get; set; }
        public bool Seen { get; set; }
        public DateTime VisitDate { get; set; }
        public VisitRequest()
        {
            SubmittedOn = DateTime.Now;
        }
    }
}
