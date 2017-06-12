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
    public class Material : ITranslatable<Material, MaterialTranslation>
    {
        [Key]
        public int Id { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<MaterialTranslation> Translations { get; set; }
        public bool AddedByAdmin { get; set; }
    }

    public class MaterialTranslation : ITranslation<Material>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public Language language { get; set; }
        [ForeignKey("material")]
        [Key, Column(Order = 1)]
        public virtual int materialId { get; set; }
        public virtual Material material { get; set; }
        public string Name { get; set; }
    }
}
