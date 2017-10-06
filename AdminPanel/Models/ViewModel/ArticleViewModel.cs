using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Models.ViewModel
{
    public class ArticleViewModel
    {
        public ArticleViewModel(Article model, string languageId)
        {
            var translation = model.Translations.SingleOrDefault(x => x.languageId == languageId);
            this.Id = model.Id;
            this.languageId = languageId;
            this.Description = translation.Description;
            this.Title = translation.Title;
            this.TitleDef = model.Title;
            this.Category = model.Category.Id;
            this.ShortDescription = translation.ShortDescription;
        }

        public ArticleViewModel()
        {

        }

        public virtual int Id { get; set; }
        [Required]
        public string languageId { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string TitleDef { get; set; }
        public List<Article> ReletedArticles { get; set; }
        public int Category { get; set; }        
        public string ThumbnailPath { get; set; }
        public Dictionary<int, string> HeaderPhotoPaths { get; set; }
    }
}