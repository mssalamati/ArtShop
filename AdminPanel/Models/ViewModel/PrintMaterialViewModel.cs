using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanel.Models.ViewModel
{
    public class PrintMaterialViewModel
    {
        public int Id { get; set; }
        public string title { get; set; }
        public decimal price { get; set; }
        public int parentid { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string color { get; set; }
        public string size { get; set; }
        public List<PrintMaterialTranslationViewModel> Translations { get; set; }
    }

    public class PrintMaterialTranslationViewModel
    {
        public string languageId { get; set; }
        public string title { get; set; }
    }
} 