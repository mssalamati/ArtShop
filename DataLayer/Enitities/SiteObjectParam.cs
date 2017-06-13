using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enitities
{
    public class SiteObjectParam
    {
        [Key]
        public int Id { get; set; }
        public string SiteName { get; set; }
        public string SliderImage { get; set; }
        public virtual ICollection<NavigationCategory> Navigations { get; set; }
    }

    public class NavigationCategory
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("category")]
        public int categoryId { get; set; }
        public virtual Category category { get; set; }

        public virtual ICollection<NavigationCategoryFavStyle> FavStyles { get; set; }
        public virtual ICollection<NavigationCategoryFavMedium> FavMediums { get; set; }
        public virtual ICollection<NavigationCategorySubject> FavSubjects { get; set; }
    }

    public class NavigationCategoryFavStyle
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("style")]
        public int styleId { get; set; }
        public virtual Style style { get; set; }
    }

    public class NavigationCategoryFavMedium
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("medium")]
        public int mediumId { get; set; }
        public virtual Medium medium { get; set; }
    }

    public class NavigationCategorySubject
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("subject")]
        public int subjectId { get; set; }
        public virtual Subject subject { get; set; }
    }
}
