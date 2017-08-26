#region Usings
using Blog.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
#endregion

namespace Blog.Objects
{
    /// <summary>
    /// Represents a tag that is labelled on a post.
    /// </summary>
    public class Tag : ITranslatable<Tag, TagTranslation>
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Name: Field is required")]
        [StringLength(500, ErrorMessage = "Name: Length should not exceed 500 characters")]
        public virtual string Name { get; set; }


        public virtual string UrlSlug { get; set; }

        public virtual string Description { get; set; }

        [JsonIgnore]
        public virtual IList<Post> Posts { get; set; }
        public virtual ICollection<TagTranslation> Translations { get; set; }

        public override string ToString()
        {
            return string.Join(",", Translations.Select(x => x.Name));
        }
    }

    public class TagTranslation : ITranslation<Tag>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [Key, Column(Order = 1)]
        [ForeignKey("tag")]
        public virtual int tagId { get; set; }
        public virtual Tag tag { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
