using DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataLayer.Enitities
{

    public class SupportSubCategory : ITranslatable<SupportSubCategory, SupportSubCategoryTranslation>
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name: Field is required")]
        [StringLength(500, ErrorMessage = "Name: Length should not exceed 500 characters")]
        public virtual string Name { get; set; }
        public virtual SupportCategory supportCategory { get; set; }
        public virtual IList<Article> Articles { get; set; }
        public virtual ICollection<SupportSubCategoryTranslation> Translations { get; set; }
        
        public override string ToString()
        {
            return string.Join(",", Translations.Select(x => x.Name));
        }
        public string GenerateSlug()
        {
            string phrase = string.Format("{0}-{1}", Id, Name);

            string str = RemoveAccent(phrase).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\u0600-\u06FF\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        private string RemoveAccent(string text)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
    public class SupportSubCategoryTranslation : ITranslation<SupportSubCategory>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [Key, Column(Order = 1)]
        [ForeignKey("supportSubCategory")]
        public virtual int SupportSubCategoryId { get; set; }
        public virtual SupportSubCategory supportSubCategory { get; set; }
        public string Name { get; set; }
    }

}

