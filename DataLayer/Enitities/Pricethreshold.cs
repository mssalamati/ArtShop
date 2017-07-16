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
    public class Pricethreshold : ITranslatable<Pricethreshold, PricethresholdTranslation>
    {
        public int Id { get; set; }
        public int? min { get; set; }
        public int? max { get; set; }
        [NotMapped]
        public string Name { get; set; }
        public virtual ICollection<PricethresholdTranslation> Translations { get; set; }
    }

    public class PricethresholdTranslation : ITranslation<Pricethreshold>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [ForeignKey("pricethreshold")]
        [Key, Column(Order = 1)]
        public virtual int pricethresholdId { get; set; }
        public virtual Pricethreshold pricethreshold { get; set; }
        public string Name { get; set; }
    }
}
