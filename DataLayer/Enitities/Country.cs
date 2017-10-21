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
        A = 0,
        B = 1,
        C = 2,
        D = 3,
        E = 4,
        F = 5,
        G = 6,
        H = 7,
        I = 8,
        J = 9,
        K = 10
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
