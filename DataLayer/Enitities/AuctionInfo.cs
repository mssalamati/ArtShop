using DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace DataLayer.Enitities
{
    public class AuctionInfo: ITranslatable<AuctionInfo, AuctionInfoTranslation>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime EndTimestamp { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<AuctionInfoTranslation> Translations { get; set; }        
        public virtual ICollection<Listing> Listings { get; set; }
        [NotMapped]
        public virtual string StartTime { get; set; }
        [NotMapped]
        public virtual string EndTime { get; set; }

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
    public class AuctionInfoTranslation : ITranslation<AuctionInfo>
    {
        [Key, Column(Order = 0)]
        [ForeignKey("language")]
        public string languageId { get; set; }
        public virtual Language language { get; set; }
        [ForeignKey("auctionInfo")]
        [Key, Column(Order = 1)]
        public virtual int Id { get; set; }
        public virtual AuctionInfo auctionInfo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}