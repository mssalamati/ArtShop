using DataLayer.Enitities;
using System;
using System.Collections.Generic;
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
    }
}