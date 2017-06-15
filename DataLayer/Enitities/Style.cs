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
    public class Style : ITranslatable<Style, StyleTranslation>
    {
        [Key]
        public int Id { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<StyleTranslation> Translations { get; set; }
        public bool AddedByAdmin { get; set; }
        public DateTime insertDate { get; set; }
        public Style()
        {
            insertDate = DateTime.Now;
        }
    }

    public class StyleTranslation : ITranslation<Style>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [ForeignKey("style")]
        [Key, Column(Order = 1)]
        public virtual int styleId { get; set; }
        public virtual Style style { get; set; }
        public string Name { get; set; }
    }
}
