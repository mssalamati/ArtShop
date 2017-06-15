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
        [ForeignKey("photo")]
        public int photoId { get; set; }
        public virtual Photo photo { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ArtCreatedDate { get; set; }
        public bool ISOrginal { get; set; }
        public bool IsPrintAvaibled { get; set; }
        public virtual Category category { get; set; }
        [ForeignKey("category")]
        public int categoryId { get; set; }
        public virtual Subject subject { get; set; }
        [ForeignKey("subject")]
        public int subjectId { get; set; }
        public string Keywords { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
        public float Depth { get; set; }
        public virtual ICollection<Style> Styles { get; set; }
        public virtual ICollection<Material> Materials { get; set; }
        public virtual ICollection<Medium> Mediums { get; set; }

        public Product()
        {
            CreateDate = DateTime.Now;
        }
    }

    public class ProductTranslation : ITranslation<Product>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [Key, Column(Order = 1)]
        public virtual int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
