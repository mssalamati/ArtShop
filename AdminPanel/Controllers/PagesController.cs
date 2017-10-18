using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class PagesController : BaseController
    {
        public ActionResult Index()
        {
            return View(db.SitePages.ToList());
        }

        public ActionResult Edit(int id, string language = "en")
        {
            ViewBag.language = db.Languages.ToList();
            var model = db.SitePages.Find(id);
            var curr = model.Translations.SingleOrDefault(x => x.languageId == language);
            if (curr == null)
            {
                var newcur = new SitePageTranslation() { languageId = language };
                model.Translations.Add(newcur);
                db.SaveChanges();
                return View(newcur);
            }
            return View(curr);
        }

        [HttpPost]
        public ActionResult Edit(SitePageTranslation model)
        {
           var finded =  db.SitePageTranslations
                .SingleOrDefault(x => x.languageId == model.languageId 
                && x.sitePageId == model.sitePageId);
            finded.Title = model.Title;
            finded.Content = model.Content;
            db.SaveChanges();
            ViewBag.language = db.Languages.ToList();
            ViewBag.message = "تغییرات با موفقیت انجام شد";
            return View(finded);
        }

        public class imagesviewmodel
        {
            public string Url { get; set; }
        }

        public void uploadnow(HttpPostedFileWrapper upload)
        {
            if (upload != null)
            {
                string ImageName = upload.FileName;
                string path = System.IO.Path.Combine(Server.MapPath("~/Uploads/cms"), ImageName);
                upload.SaveAs(path);
            }
        }

        public ActionResult uploadPartial()
        {
            var appData = Server.MapPath("~/Uploads/cms");
            var images = Directory.GetFiles(appData).Select(x => new imagesviewmodel
            {
                Url = Url.Content("https://file.artiscovery.com/Uploads/cms/" + Path.GetFileName(x))
            });
            return View(images);
        }
    }
}