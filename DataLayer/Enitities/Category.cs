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
    public class Category : ITranslatable<Category, CategoryTranslation>
    {
        [Key]
        public int Id { get; set; }
        public virtual ICollection<CategoryTranslation> Translations { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        [ForeignKey("photo")]
        public int photoId { get; set; }
        public virtual Photo photo { get; set; }
    }

    public class CategoryTranslation : ITranslation<Category>
    {      
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [Key, Column(Order = 1)]
        [ForeignKey("category")]
        public virtual int categoryId { get; set; }
        public virtual Category category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
