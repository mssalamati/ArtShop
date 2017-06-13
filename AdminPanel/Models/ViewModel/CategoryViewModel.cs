using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanel.Models.ViewModel
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public virtual ICollection<CategoryTranslationViewModel> Translations { get; set; }
    }

    public class CategoryTranslationViewModel
    {
        public string languageId { get; set; }
        public virtual int categoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}