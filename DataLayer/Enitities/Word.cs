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
    public class WordCategory
    {
        [Key]
        public String Name { get; set; }
        public virtual ICollection<Word> Words { get; set; }
    }

    public class Word : ITranslatable<Word, WordTranslation>
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("category")]
        public virtual String categoryId { get; set; }
        public virtual WordCategory category { get; set; }
        public string Name { get; set; }
        public virtual ICollection<WordTranslation> Translations { get; set; }
    }

    public class WordTranslation : ITranslation<Word>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [ForeignKey("word")]
        [Key, Column(Order = 1)]
        public virtual int wordId { get; set; }
        public virtual Word word { get; set; }
        public string Name { get; set; }
    }
}
