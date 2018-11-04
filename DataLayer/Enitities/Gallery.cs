using DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataLayer.Enitities
{
    public class Gallery: ITranslatable<Gallery, GalleryTranslation>
    {
        public int Id { get; set; }
        public string DefaultTitle { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime EndTimestamp { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<GalleryTranslation> Translations { get; set; }        

        [NotMapped]
        public virtual string StartTime { get; set; }
        [NotMapped]
        public virtual string EndTime { get; set; }
    }
    public class GalleryTranslation : ITranslation<Gallery>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [ForeignKey("gallery")]
        [Key, Column(Order = 1)]
        public virtual int Id { get; set; }
        public virtual Gallery gallery { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Content { get; set; }
    }
}
