using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanel.Models.ViewModel
{
    public class ParamsViewModel
    {
        public string name { get; set; }
        public virtual ICollection<ParamsViewModelTranslation> Translations { get; set; }
    }
    public class ParamsViewModelTranslation
    {
        public string languageId { get; set; }
        public string Name { get; set; }
    }
}