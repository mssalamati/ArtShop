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
            this.Category = model.SupportCategory.Id;
            this.ThumbnailPath = "/" + model.Thumbnail;
            this.ReletedArticles = model.ReletedArticles.Select(x => x.Id).ToList();
            this.SubCategory = model.SupportSubCategory != null ? model.SupportSubCategory.Id : 0;
            this.ShortDescription = translation.ShortDescription;
            this.isHandbook = model.isHandbook;
        }

        public ArticleViewModel()
        {

        }

        public ArticleViewModel FillPicture(Article model)
        {
            ThumbnailPath = "/" + model.Thumbnail;
            return this;
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
        public bool isHandbook { get; set; }
        public CategoryType ArticleType { get; set; }
        public List<int> ReletedArticles { get; set; }
        public int Category { get; set; }
        public int SubCategory { get; set; }
        public HttpPostedFileBase Thumbnail { get; set; }
        public string ThumbnailPath { get; set; }
        public Dictionary<int, string> HeaderPhotoPaths { get; set; }
    }
}