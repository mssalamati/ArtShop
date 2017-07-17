using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enitities
{
    public class ShoppingCart
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual decimal Quantity { get; set; }
        public virtual Product Product { get; set; }
        [Index("IX_Cart", IsUnique = true, Order = 1)]
        public virtual int ProductId { get; set; }
        [Index("IX_Cart", IsUnique = true, Order = 2)]
        public virtual string CartNumber { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual Ordertype type { get; set; }
    }
}
