using AdminPanel.Models.ViewModel;
using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class SupportSubCategoriesController : BaseController
    {
        // GET: SupportSubCategories
        public ActionResult Index(int page = 1, string search = "")
        {
            int count = 0, pagesize = 15, take = pagesize, skip = (page - 1) * pagesize;
            var data = db.SupportSubCategories
                 .Where(x => string.IsNullOrEmpty(search) || x.Name.Contains(search))
                 .OrderByDescending(x => x.Name)
                 .Skip(skip).Take(take);
            count = db.SupportSubCategories.Count();
            int maxpage = count % pagesize != 0 ? (count / pagesize) + 1 : (count / pagesize);
            ViewBag.page = page; ViewBag.maxpage = maxpage; ViewBag.search = search;

            return View(data.ToList());
        }
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Add()
        {
            ViewBag.language = db.Languages.ToList();
            ViewBag.categories = db.SupportCategories.ToList();
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(SupportCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.language = db.Languages.ToList();
                ViewBag.categories = db.SupportCategories.ToList();
                return PartialView(model);
            }

            var supportCat = db.SupportCategories.Find(model.supportCategoryId);
            SupportSubCategory c = new SupportSubCategory() { Name = model.Name,supportCategory = supportCat};
            
            supportCat.supportSubCategories.Add(c);
            c.Translations = new List<SupportSubCategoryTranslation>();
            foreach (var item in model.Translations)
                c.Translations.Add(new SupportSubCategoryTranslation() { languageId = item.languageId, Name = item.Name});
            db.SupportSubCategories.Add(c);
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
            var finder = db.SupportSubCategories.Find(id);
            ViewBag.language = db.Languages.ToList();
            ViewBag.categories = db.SupportCategories.ToList();
            SupportCategoryViewModel cvm = new SupportCategoryViewModel() { Id = finder.Id, Translations = new List<SupportCategoryTranslationViewModel>(), Name = finder.Name};
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new SupportCategoryTranslationViewModel() { languageId = item.languageId, Name = item.Name});

            return PartialView(cvm);
        }

        [HttpPost]
        public ActionResult Edit(SupportCategoryViewModel model)
        {
            var finder = db.SupportSubCategories.Find(model.Id);
            finder.Name = model.Name;
            var supportCat = db.SupportCategories.Find(model.supportCategoryId);
            finder.supportCategory = supportCat;

            foreach (var item in model.Translations)
            {
                var curr = finder.Translations.SingleOrDefault(x => x.languageId == item.languageId);
                if (curr != null)
                {
                    curr.Name = item.Name;
                }

                else
                    finder.Translations.Add(new SupportSubCategoryTranslation() { languageId = item.languageId, Name = item.Name});

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
            ViewBag.categories = db.SupportCategories.ToList();
            SupportCategoryViewModel cvm = new SupportCategoryViewModel() { Id = finder.Id, Translations = new List<SupportCategoryTranslationViewModel>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new SupportCategoryTranslationViewModel() { languageId = item.languageId, Name = item.Name });
            return PartialView(cvm);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var finder = db.SupportSubCategories.Find(id);
            db.SupportSubCategories.Remove(finder);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}