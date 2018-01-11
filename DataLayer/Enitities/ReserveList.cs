using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enitities
{
    public class ReserveList
    {
        public int Id { get; set; }
        public int quantity { get; set; }
        [ForeignKey("product")]
        public int ProductId { get; set; }
        public virtual Product product { get; set; }
        public DateTime datetime { get; set; }
        public ReserveList()
        {
            datetime = DateTime.Now;
        }
        public ReserveList(int quantity)
        {
            this.quantity = quantity;
            datetime = DateTime.Now;
        }
    }
}
