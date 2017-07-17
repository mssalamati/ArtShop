using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanel.Models.ViewModel
{
    public class PriceListViewModel
    {
        public int Id { get; set; }
        public int? min { get; set; }
        public int? max { get; set; }
        public List<PriceListTranslationViewModel> Translations { get; set; }
    }
    public class PriceListTranslationViewModel
    {
        public string languageId { get; set; }
        public int countryId { get; set; }
        public string Name { get; set; }
    }
}