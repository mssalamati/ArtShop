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
    public class Subject : ITranslatable<Subject, SubjectTranslation>
    {
        public virtual int Id { get; set; }
        public virtual ICollection<SubjectTranslation> Translations { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }

    public class SubjectTranslation : ITranslation<Subject>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public Language language { get; set; }
        [ForeignKey("subject")]
        [Key, Column(Order = 1)]
        public virtual int subjectId { get; set; }
        public virtual Subject subject { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
