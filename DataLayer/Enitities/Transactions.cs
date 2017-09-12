using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enitities
{
    public class Transaction
    {
        public virtual int Id { get; set; }
        [ForeignKey("user")]
        public virtual string user_id { get; set; }
        public virtual UserProfile user { get; set; }
        [ForeignKey("order")]
        public virtual int? orderId { get; set; }
        public virtual Order order { get; set; }
        public string Detail { get; set; }
        public string Amount { get; set; }
        public bool isIncreased { get; set; }
        public DateTime date { get; set; }
    }
}
