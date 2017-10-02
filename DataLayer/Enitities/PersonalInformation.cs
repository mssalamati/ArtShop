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
    public class PersonalInformation : ITranslatable<PersonalInformation, PersonalInformationTranslation>
    {
        [Key]
        public virtual int Id { get; set; }
        //public virtual string AboutMe { get; set; }
        //public virtual string Education { get; set; }
        //public virtual string Events { get; set; }
        //public virtual string Exhibitions { get; set; }
        public virtual ICollection<PersonalInformationTranslation> Translations { get; set; }
    }
    public class PersonalInformationTranslation : ITranslation<PersonalInformation>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [Key, Column(Order = 1)]
        [ForeignKey("personalInformation")]
        public virtual int personalInformationId { get; set; }
        public virtual PersonalInformation personalInformation { get; set; }
        public virtual string AboutMe { get; set; }
        public virtual string Education { get; set; }
        public virtual string Events { get; set; }
        public virtual string Exhibitions { get; set; }
    }
}
