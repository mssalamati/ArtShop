using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Models.ViewModel
{
    public class GalleryViewModel
    {
        public GalleryViewModel(Gallery model, string languageId)
        {
            var translation = model.Translations.SingleOrDefault(x => x.languageId == languageId);
            this.Id = model.Id;
            this.languageId = languageId;
            this.Content = translation.Content;
            this.Title = translation.Title;
            this.Title = model.DefaultTitle;
            this.Active = model.Active;
            this.StartTime = model.StartTime;
            this.EndTime = model.EndTime;
            this.StartTimestamp = model.StartTimestamp;
            this.EndTimestamp = model.EndTimestamp;
        }

        public GalleryViewModel()
        {

        }

        public virtual int Id { get; set; }
        [Required]
        public string languageId { get; set; }

        [AllowHtml]
        public string Content { get; set; }
        [Required]
        public string Title { get; set; }       
        public virtual string StartTime { get; set; }
        public bool Active { get; set; }
        public virtual string EndTime { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime EndTimestamp { get; set; }

    }
}
