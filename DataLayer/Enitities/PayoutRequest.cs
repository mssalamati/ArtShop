using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enitities
{
    public class PayoutRequest
    {
        public int Id { get; set; }
        [ForeignKey("user")]
        public virtual String user_id { get; set; }
        [Display(Name = "کاربر")]
        public virtual UserProfile user { get; set; }
        public virtual DateTime date { get; set; }
        public decimal Value { get; set; }
        public bool Payed { get; set; }
        public bool Seen { get; set; }
        public string CardNumber { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountHolder { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
