using Blog.Areas.Admin.Models.ViewModel;
using Blog.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Areas.Admin.Controllers
{
    public class CategoriesController : BaseController
    {
        // GET: Admin/Categories
        public ActionResult Index(int page = 1, string search = "")
        {
            var data = db.Categories;
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
        public ActionResult Add(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.language = db.Languages.ToList();
                return PartialView(model);
            }


            Category c = new Category() { Name = model.Name };

            c.Translations = new List<CategoryTranslation>();
            foreach (var item in model.Translations)
                c.Translations.Add(new CategoryTranslation() { languageId = item.languageId, Name = item.Name });
            db.Categories.Add(c);
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
            var finder = db.Categories.Find(id);
            ViewBag.language = db.Languages.ToList();
            CategoryViewModel cvm = new CategoryViewModel() { Id = finder.Id, Translations = new List<CategoryTranslationViewModel>(), Name = finder.Name };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new CategoryTranslationViewModel() { languageId = item.languageId, Name = item.Name });

            return PartialView(cvm);
        }

        [HttpPost]
        public ActionResult Edit(CategoryViewModel model)
        {
            var finder = db.Categories.Find(model.Id);


            foreach (var item in model.Translations)
            {
                var curr = finder.Translations.SingleOrDefault(x => x.languageId == item.languageId);
                if (curr != null)
                    curr.Name = item.Name;
                else
                    finder.Translations.Add(new CategoryTranslation() { languageId = item.languageId, Name = item.Name });
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
            CategoryViewModel cvm = new CategoryViewModel() { Id = finder.Id, Translations = new List<CategoryTranslationViewModel>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new CategoryTranslationViewModel() { languageId = item.languageId, Name = item.Name });
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