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
    public class Medium : ITranslatable<Medium, MediumTranslation>
    {
        [Key]
        public int Id { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<MediumTranslation> Translations { get; set; }
        public bool AddedByAdmin { get; set; }
    }

    public class MediumTranslation : ITranslation<Medium>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [ForeignKey("medium")]
        [Key, Column(Order = 1)]
        public virtual int mediumId { get; set; }
        public virtual Medium medium { get; set; }
        public string Name { get; set; }
    }

}
