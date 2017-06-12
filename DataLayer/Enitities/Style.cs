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
    }

    public class StyleTranslation : ITranslation<Style>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public Language language { get; set; }
        [Key, Column(Order = 1)]
        public string Name { get; set; }
    }
}
