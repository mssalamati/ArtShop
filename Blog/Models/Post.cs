#region Usings
using Blog.Interfaces;
using Blog.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
#endregion

namespace Blog.Objects
{
    public enum PostType { Wide, Sqr, ShowCase }
    /// <summary>
    /// Represents a blog entry - article, presentation or any thing.
    /// </summary>
    public class Post : ITranslatable<Post, PostTranslation>
    {
        public virtual int Id { get; set; }

        [ForeignKey("AuthorProfile")]
        public string AuthorProfileId { get; set; }
        public virtual UserProfile AuthorProfile { get; set; }

        /// <summary>
        /// The heading of the post.
        /// </summary>
        [Required(ErrorMessage = "Title: Field is required")]
        [StringLength(500, ErrorMessage = "Title: Length should not exceed 500 characters")]
        public virtual string Title { get; set; }


        public virtual ICollection<PostTranslation> Translations { get; set; }

        /// <summary>
        /// The information about the post that has to be displayed in the &lt;meta&gt; tag (SEO).
        /// </summary>
        /// <remarks>
        /// Not sure Google still uses this for ranking but other search providers might be.
        /// </remarks>
        [StringLength(1000, ErrorMessage = "Meta: Length should not exceed 1000 characters")]
        public virtual string Meta { get; set; }

        /// <summary>
        /// The url slug that is used to define the post address.
        /// </summary>
        [StringLength(1000, ErrorMessage = "Meta: UrlSlug should not exceed 50 characters")]
        public virtual string UrlSlug { get; set; }

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

        /// <summary>
        /// Collection of tags labelled over the post.
        /// </summary>
        public virtual IList<Tag> Tags { get; set; }
        public virtual IList<Link> Links { get; set; }
        public virtual IList<HeaderPhoto> HeaderPhotos { get; set; }
        public virtual string Author { get; set; }
        public virtual string Thumbnail { get; set; }
        public virtual PostType postType { get; set; }

        public Post()
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

    public class PostTranslation : ITranslation<Post>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [Key, Column(Order = 1)]
        [ForeignKey("post")]
        public virtual int postId { get; set; }
        public virtual Post post { get; set; }
        public virtual string Description { get; set; }
        public virtual string ShortDescription { get; set; }
        public virtual string Title { get; set; }
    }
}
