using DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enitities
{
    public class Product : ITranslatable<Product, ProductTranslation>
    {
        public virtual int Id { get; set; }
        public virtual ICollection<ProductTranslation> Translations { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ArtCreatedDate { get; set; }
        public bool ISOrginal { get; set; }
        public bool IsPrintAvaibled { get; set; }
    }

    public class ProductTranslation : ITranslation<Product>
    {
        [Key, Column(Order = 0)]
        public string languageId { get; set; }
        public Language language { get; set; }
        [Key, Column(Order = 1)]
        public virtual int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
