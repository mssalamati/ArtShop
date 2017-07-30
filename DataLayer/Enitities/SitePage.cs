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
    public class SitePage : ITranslatable<SitePage, SitePageTranslation>
    {
        public int Id { get; set; }
        public string DefaultTitle { get; set; }
        public virtual ICollection<SitePageTranslation> Translations { get; set; }
    }

    public class SitePageTranslation : ITranslation<SitePage>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [ForeignKey("sitePage")]
        [Key, Column(Order = 1)]
        public virtual int sitePageId { get; set; }
        public virtual SitePage sitePage { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Content { get; set; }
    }
}
