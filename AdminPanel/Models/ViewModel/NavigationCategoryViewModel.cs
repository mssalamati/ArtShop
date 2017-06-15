using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanel.Models.ViewModel
{
    public class NavigationCategoryViewModel
    {
        public int Id { get; set; }
        public List<int> FavStyles { get; set; }
        public List<int> FavMediums { get; set; }
        public List<int> FavSubjects { get; set; }
    }
}