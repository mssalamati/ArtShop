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
    public class SiteParam : ITranslatable<SiteParam, SiteParamTranslation>
    {
        [Key]
        public string Name { get; set; }

        public virtual ICollection<SiteParamTranslation> Translations { get; set; }
    }

    public class SiteParamTranslation : ITranslation<SiteParam>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public Language language { get; set; }

        [Key, Column(Order = 1)]
        public virtual int siteParamId { get; set; }
        public virtual SiteParam siteParam { get; set; }

        public string Value { get; set; }
    }
}
