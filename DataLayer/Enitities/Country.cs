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
    public class Country : ITranslatable<Country, CountryTranslation>
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public CountryRegion region { get; set; }
        public virtual ICollection<CountryTranslation> Translations { get; set; }
        public virtual ICollection<UserProfile> users { get; set; }
    }

    public enum CountryRegion
    {
        A = 1,
        B = 2,
        C = 3,
        D = 4,
        E = 5,
        F = 6,
        G = 7,
        H = 8,
        I = 9,
        J = 10
    }

    public class CountryTranslation : ITranslation<Country>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [ForeignKey("country")]
        [Key, Column(Order = 1)]
        public virtual int countryId { get; set; }
        public virtual Country country { get; set; }
        public string Name { get; set; }
    }
}
