using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanel.Models.ViewModel
{
    public class ProductTypeViewModel
    {
        public int Id { get; set; }
        public bool AddedByAdmin { get; set; }
        public virtual ICollection<ProductTypeTranslationViewModel> Translations { get; set; }
    }

    public class ProductTypeTranslationViewModel
    {
        public string languageId { get; set; }
        public virtual int categoryId { get; set; }
        public string Name { get; set; }
    }
}