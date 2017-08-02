using DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Extentions;

namespace DataLayer.Enitities
{
    public class PrintMaterial : ITranslatable<PrintMaterial, PrintMaterialTranslation>
    {
        public int Id { get; set; }
        public string title { get; set; }
        public virtual ICollection<PrintSize> PrintSizes { get; set; }
        public virtual ICollection<PrintMaterialTranslation> Translations { get; set; }

        public string toJson()
        {
            string json = string.Empty;
            json += "[";
            json += string.Join(",", PrintSizes.Select(item => "{" +
             "\"width\":\"" + item.Width + "\" ,\"height\":\"" + item.Height + "\" ,\"id\":\"" + item.Id + "\" ,\"title\":\"" + item.Current().title + "\",\"price\":\"" + item.price + "\",\"frame\":"
                    + "[" + string.Join(",", item.PrintFrames.Select(x => "{\"color\":\"" + x.color + "\",\"size\":\"" + x.size + "\",\"price\":\"" + x.price + "\",\"val\":\"" + x.Id + "\",\"desc\":\"" + x.Current().title + "\"}")) + "]" +
            "}"));
            json += "]";
            return json;
        }
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
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class PrintFrame : ITranslatable<PrintFrame, PrintFrameTranslation>
    {
        public int Id { get; set; }
        [ForeignKey("printSize")]
        public virtual int printSizeId { get; set; }
        public virtual PrintSize printSize { get; set; }
        public virtual ICollection<PrintFrameTranslation> Translations { get; set; }
        public string title { get; set; }
        public string color { get; set; }
        public string size { get; set; }
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
