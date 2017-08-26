using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Areas.Admin.Models.ViewModel
{
    public class TagViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<TagTranslationViewModel> Translations { get; set; }
    }

    public class TagTranslationViewModel
    {
        public string languageId { get; set; }
        public virtual int tagId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}