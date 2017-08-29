using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Objects;

namespace Blog.Areas.Admin.Models.ViewModel
{
    public class PostViewModel
    {

        public PostViewModel(Post model, string languageId)
        {
            var translation = model.Translations.SingleOrDefault(x => x.languageId == languageId);
            this.Id = model.Id;
            this.languageId = languageId;
            this.Description = translation.Description;
            this.Title = translation.Title;
            this.TitleDef = model.Title;
            this.Tags = model.Tags.Select(x => x.Id).ToList();
            this.Links = model.Links.Select(x => x.URL).ToList();
            this.Category = model.Category.Id;
            this.ThumbnailPath = "/" + model.Thumbnail;
            this.HeaderPhotoPaths = model.HeaderPhotos.ToDictionary(x => x.Id, x => "/" + x.Path);
        }

        public PostViewModel()
        {

        }

        public PostViewModel FillPicture(Post model)
        {
            ThumbnailPath = "/" + model.Thumbnail;
            HeaderPhotoPaths = model.HeaderPhotos.ToDictionary(x => x.Id, x => "/" + x.Path);
            return this;
        }

        public virtual int Id { get; set; }
        [Required]
        public string languageId { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string TitleDef { get; set; }
        [LimitCount(1, 50, ErrorMessage = "tags between 1 and 50")]
        public List<int> Tags { get; set; }
        public List<string> Links { get; set; }
        public int Category { get; set; }
        public HttpPostedFileBase Thumbnail { get; set; }
        public List<HttpPostedFileBase> HeaderPhotos { get; set; }


        public string ThumbnailPath { get; set; }
        public Dictionary<int, string> HeaderPhotoPaths { get; set; }
    }
}