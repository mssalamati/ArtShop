using AdminPanel.Models.ViewModel;
using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class SupportCategoriesController : BaseController
    {
        // GET: SupportCategories
        public ActionResult Index(int page = 1, string search = "")
        {
            int count = 0, pagesize = 15, take = pagesize, skip = (page - 1) * pagesize;
            var data = db.SupportCategories
                 .Where(x => string.IsNullOrEmpty(search) || x.Name.Contains(search))
                 .OrderByDescending(x => x.Name)
                 .Skip(skip).Take(take);
            count = data.Count();
            int maxpage = count % pagesize != 0 ? (count / pagesize) + 1 : (count / pagesize);
            ViewBag.page = page; ViewBag.maxpage = maxpage; ViewBag.search = search;

            return View(data.ToList());
        }
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Add()
        {
            ViewBag.language = db.Languages.ToList();
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(SupportCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.language = db.Languages.ToList();
                return PartialView(model);
            }

            SupportCategory c = new SupportCategory() { Name = model.Name, Thumbnail = model.Thumbnail, categoryType = model.categorytype };

            c.Translations = new List<SupportCategoryTranslation>();
            foreach (var item in model.Translations)
                c.Translations.Add(new SupportCategoryTranslation() { languageId = item.languageId, Name = item.Name, Description = item.Description });
            db.SupportCategories.Add(c);
            try
            {
                db.SaveChanges();
                return PartialView("_successWindow");
            }
            catch (Exception ex)
            {
                ViewBag.language = db.Languages.ToList();
                ModelState.AddModelError(string.Empty, ex.ToString());
                return PartialView(model);
            }
        }


        public ActionResult Edit(int id)
        {
            var finder = db.SupportCategories.Find(id);
            ViewBag.language = db.Languages.ToList();
            SupportCategoryViewModel cvm = new SupportCategoryViewModel() { Id = finder.Id, Translations = new List<SupportCategoryTranslationViewModel>(), Name = finder.Name, categorytype = finder.categoryType, Thumbnail = finder.Thumbnail };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new SupportCategoryTranslationViewModel() { languageId = item.languageId, Name = item.Name, Description = item.Description });

            return PartialView(cvm);
        }

        [HttpPost]
        public ActionResult Edit(SupportCategoryViewModel model)
        {
            var finder = db.SupportCategories.Find(model.Id);
            finder.Name = model.Name;
            finder.categoryType = model.categorytype;
            finder.Thumbnail = model.Thumbnail;

            foreach (var item in model.Translations)
            {
                var curr = finder.Translations.SingleOrDefault(x => x.languageId == item.languageId);
                if (curr != null)
                {
                    curr.Name = item.Name;
                    curr.Description = item.Description;
                }

                else
                    finder.Translations.Add(new SupportCategoryTranslation() { languageId = item.languageId, Name = item.Name, Description = item.Description });

            }

            try
            {
                db.SaveChanges();
                return PartialView("_successWindow");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.ToString());
            }

            ViewBag.language = db.Languages.ToList();
            SupportCategoryViewModel cvm = new SupportCategoryViewModel() { Id = finder.Id, Translations = new List<SupportCategoryTranslationViewModel>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new SupportCategoryTranslationViewModel() { languageId = item.languageId, Name = item.Name });
            return PartialView(cvm);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var finder = db.Categories.Find(id);
            db.Categories.Remove(finder);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}