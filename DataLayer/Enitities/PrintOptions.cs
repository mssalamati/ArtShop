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
    public class PrintMaterial : ITranslatable<PrintMaterial, PrintMaterialTranslation>
    {
        public int Id { get; set; }
        public virtual ICollection<PrintSize> PrintSizes { get; set; }
        public virtual ICollection<PrintMaterialTranslation> Translations { get; set; }
    }

    public class PrintSize : ITranslatable<PrintSize, PrintSizeTranslation>
    {
        public int Id { get; set; }
        [ForeignKey("printMaterial")]
        public virtual int printMaterialId { get; set; }
        public virtual PrintMaterial printMaterial { get; set; }
        public virtual ICollection<PrintSizeTranslation> Translations { get; set; }
        public virtual ICollection<PrintFrame> PrintFrames { get; set; }
        public string title { get; set; }
        public decimal price { get; set; }
    }

    public class PrintFrame : ITranslatable<PrintFrame, PrintFrameTranslation>
    {
        public int Id { get; set; }
        [ForeignKey("printSize")]
        public virtual int printSizeId { get; set; }
        public virtual PrintSize printSize { get; set; }
        public virtual ICollection<PrintFrameTranslation> Translations { get; set; }
        public string title { get; set; }
        public decimal price { get; set; }
    }

    public class PrintFrameTranslation : ITranslation<PrintFrame>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [ForeignKey("printFrame")]
        [Key, Column(Order = 1)]
        public virtual int printFrameId { get; set; }
        public virtual PrintFrame printFrame { get; set; }
        public string title { get; set; }
    }

    public class PrintSizeTranslation : ITranslation<PrintSize>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [ForeignKey("printSize")]
        [Key, Column(Order = 1)]
        public virtual int printSizeId { get; set; }
        public virtual PrintSize printSize { get; set; }
        public string title { get; set; }
    }

    public class PrintMaterialTranslation : ITranslation<PrintMaterial>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [ForeignKey("printMaterial")]
        [Key, Column(Order = 1)]
        public virtual int printMaterialId { get; set; }
        public virtual PrintMaterial printMaterial { get; set; }
        public string title { get; set; }
    }
}
