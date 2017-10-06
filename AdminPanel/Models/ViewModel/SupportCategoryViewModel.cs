using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanel.Models.ViewModel
{
    public class SupportCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<SupportCategoryTranslationViewModel> Translations { get; set; }
    }

    public class SupportCategoryTranslationViewModel
    {
        public string languageId { get; set; }
        public virtual int categoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

}