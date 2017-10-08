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
    public class Article : ITranslatable<Article, ArticleTranslation>
    {
        public virtual int Id { get; set; }

        [ForeignKey("AuthorProfile")]
        public string AuthorProfileId { get; set; }
        public virtual UserProfile AuthorProfile { get; set; }

        public ICollection<Article> ReletedArticles { get; set; }
        /// <summary>
        /// The heading of the post.
        /// </summary>
        [Required(ErrorMessage = "Title: Field is required")]
        [StringLength(500, ErrorMessage = "Title: Length should not exceed 500 characters")]
        public virtual string Title { get; set; }

        public virtual ICollection<ArticleTranslation> Translations { get; set; }
        public virtual CategoryType ArticleType { get; set; }
        /// <summary>
        /// The information about the post that has to be displayed in the &lt;meta&gt; tag (SEO).
        /// </summary>
        /// <remarks>
        /// Not sure Google still uses this for ranking but other search providers might be.
        /// </remarks>
        [StringLength(1000, ErrorMessage = "Meta: Length should not exceed 1000 characters")]
        public virtual string Meta { get; set; }

        /// <summary>
        /// Flag to represent whether the article is published or not.
        /// </summary>
        public virtual bool Published { get; set; }

        /// <summary>
        /// The post published date.
        /// </summary>
        [Required(ErrorMessage = "PostedOn: Field is required")]
        public virtual DateTime PostedOn { get; set; }

        /// <summary>
        /// The post's last modified date.
        /// </summary>
        public virtual DateTime? Modified { get; set; }

        /// <summary>
        /// The category to which the post belongs to.
        /// </summary>
        public virtual Category Category { get; set; }

        public virtual string Author { get; set; }
        public virtual string Thumbnail { get; set; }

        public Article()
        {
            PostedOn = DateTime.Now;
        }

        public string GenerateSlug()
        {
            string phrase = string.Format("{0}-{1}", Id, Title);

            string str = phrase.ToLower();//RemoveAccent(phrase).ToLower();
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

    public class ArticleTranslation : ITranslation<Article>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [Key, Column(Order = 1)]
        [ForeignKey("article")]
        public virtual int articleId { get; set; }
        public virtual Article article { get; set; }
        public virtual string Description { get; set; }
        public virtual string ShortDescription { get; set; }
        public virtual string Title { get; set; }
    }
}
