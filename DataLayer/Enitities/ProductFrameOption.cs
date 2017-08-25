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
    public class ProductFrameOptions
    {
        public Dictionary<int, string> ProductFrameColors { get; set; }
        public Dictionary<int, string> ProductFrameMaterials { get; set; }
        public Dictionary<int, string> ProductFrameTypes { get; set; }
    }

    public class ProductFrameColor : ITranslatable<ProductFrameColor, ProductFrameColorTranslation>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ProductFrameColorTranslation> Translations { get; set; }
    }

    public class ProductFrameColorTranslation : ITranslation<ProductFrameColor>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [ForeignKey("productFrameColor")]
        [Key, Column(Order = 1)]
        public virtual int productFrameColorId { get; set; }
        public virtual ProductFrameColor productFrameColor { get; set; }
        public string Name { get; set; }
    }

    public class ProductFrameMaterial : ITranslatable<ProductFrameMaterial, ProductFrameMaterialTranslation>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ProductFrameMaterialTranslation> Translations { get; set; }
    }

    public class ProductFrameMaterialTranslation : ITranslation<ProductFrameMaterial>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [ForeignKey("productFrameMaterial")]
        [Key, Column(Order = 1)]
        public virtual int productFrameMaterialId { get; set; }
        public virtual ProductFrameMaterial productFrameMaterial { get; set; }
        public string Name { get; set; }
    }

    public class ProductFrameType : ITranslatable<ProductFrameType, ProductFrameTypeTranslation>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ProductFrameTypeTranslation> Translations { get; set; }
    }

    public class ProductFrameTypeTranslation : ITranslation<ProductFrameType>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [ForeignKey("productFrameType")]
        [Key, Column(Order = 1)]
        public virtual int productFrameTypeId { get; set; }
        public virtual ProductFrameType productFrameType { get; set; }
        public string Name { get; set; }
    }
}
