using DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enitities
{
    public class FirstPageSection
    {
        public int Id { get; set; }
        public string type { get; set; }
        public int priority { get; set; }
        public string param1 { get; set; }
        public string param2 { get; set; }
        public string param3 { get; set; }
        public string param4 { get; set; }
        public string param5 { get; set; }
        public string param6 { get; set; }
    }

    public class footerCell : ITranslatable<footerCell, footerCellTranslation>
    {
        public int Id { get; set; }
        public virtual ICollection<footerCellTranslation> Translations { get; set; }
    }

    public class footerCellTranslation : ITranslation<footerCell>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [ForeignKey("footercell")]
        [Key, Column(Order = 1)]
        public virtual int footercellId { get; set; }
        public virtual footerCell footercell { get; set; }
        public string Header { get; set; }
        public virtual ICollection<footercellRow> footercellRows { get; set; }
    }

    public class footercellRow
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string link { get; set; }
    }

    public class sliderImage : ITranslatable<sliderImage, sliderImageTranslation>
    {
        public int Id { get; set; }
        public string path { get; set; }
        public string ButtonURL { get; set; }
        public string TextColor { get; set; }
        public string ButtonColor { get; set; }
        public string ButtonTextColor { get; set; }
        public virtual ICollection<sliderImageTranslation> Translations { get; set; }
    }

    public class sliderImageTranslation : ITranslation<sliderImage>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [ForeignKey("sliderimage")]
        [Key, Column(Order = 1)]
        public virtual int sliderimageId { get; set; }
        public virtual sliderImage sliderimage { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        public string H1 { get; set; }
        public string H2 { get; set; }
        public string ButtonText { get; set; }
    }

    public class ImageServer
    {
        public int Id { get; set; }
        public string Host { get; set; }
        public bool SSAllowPicture { get; set; }
        public float diskUsage { get; set; }
        public float maxStoreage { get; set; }
        public string mainDomain { get; set; }
        public string Name { get; set; }
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
