using DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Enitities
{
    public class MobileHomePage : ITranslatable<MobileHomePage, MobileHomePageTranslation>
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string PhotoPath { get; set; }
        public virtual ICollection<MobileHomePageItem> Items { get; set; }
        public virtual ICollection<MobileHomePageTranslation> Translations { get; set; }
        [NotMapped]
        public HttpPostedFileBase Image { get; set; }
    }

    public class MobileHomePageTranslation : ITranslation<MobileHomePage>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [ForeignKey("mobileHomePage")]
        [Key, Column(Order = 1)]
        public virtual int mobileHomePageId { get; set; }
        public virtual MobileHomePage mobileHomePage { get; set; }
        public string Title { get; set; }
    }

    public class MobileHomePageItem
    {
        [Key]
        public int Id { get; set; }

        public virtual MobileHomePage mobileHomePage { get; set; }
        [ForeignKey("mobileHomePage")]
        public int mobileHomePageId { get; set; }

        public virtual Product product { get; set; }
        [ForeignKey("product")]
        public int ProductId { get; set; }
    }
}
