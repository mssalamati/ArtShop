using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Areas.Admin.Models.ViewModel
{
    public class CountryViewModel
    {
        public int Id { get; set; }
        public string code { get; set; }
        public List<CountryTranslationViewModel> Translations { get; set; }
    }
    public class CountryTranslationViewModel
    {
        public string languageId { get; set; }
        public int countryId { get; set; }
        public string Name { get; set; }
    }
}