using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Configurations
{
    public class ShoppingCartConfig : EntityTypeConfiguration<ShoppingCart>
    {
        public ShoppingCartConfig()
        {
            Property(a => a.CartNumber).IsRequired().HasMaxLength(50);
        }
    }
}
