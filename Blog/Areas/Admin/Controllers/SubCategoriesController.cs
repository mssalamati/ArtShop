using Blog.Areas.Admin.Models.ViewModel;
using Blog.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Areas.Admin.Controllers
{
    public class SubCategoriesController : BaseController
    {
        public ActionResult Index(int page = 1, string search = "")
        {
            int count = 0, pagesize = 15, take = pagesize, skip = (page - 1) * pagesize;
            var data = db.SubCategories
                 .Where(x => string.IsNullOrEmpty(search) || x.Name.Contains(search))
                 .OrderByDescending(x => x.Name)
                 .Skip(skip).Take(take);
            count = db.SubCategories.Count();
            int maxpage = count % pagesize != 0 ? (count / pagesize) + 1 : (count / pagesize);
            ViewBag.page = page; ViewBag.maxpage = maxpage; ViewBag.search = search;

            return View(data.ToList());
        }
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Add()
        {
            ViewBag.language = db.Languages.ToList();
            ViewBag.categories = db.Categories.ToList();
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.language = db.Languages.ToList();
                ViewBag.categories = db.Categories.ToList();
                return PartialView(model);
            }

            var supportCat = db.Categories.Find(model.CategoryId);
            SubCategory c = new SubCategory() { Name = model.Name, Category = supportCat };

            supportCat.SubCategories.Add(c);
            c.Translations = new List<SubCategoryTranslation>();
            foreach (var item in model.Translations)
                c.Translations.Add(new SubCategoryTranslation() { languageId = item.languageId, Name = item.Name, Description = item.Description });
            db.SubCategories.Add(c);
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
            var finder = db.SubCategories.Find(id);
            ViewBag.language = db.Languages.ToList();
            ViewBag.categories = db.Categories.ToList();
            CategoryViewModel cvm = new CategoryViewModel() { Id = finder.Id, Translations = new List<CategoryTranslationViewModel>(), Name = finder.Name, CategoryId = finder.Category.Id };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new CategoryTranslationViewModel() { languageId = item.languageId, Name = item.Name, Description = item.Description });

            return PartialView(cvm);
        }

        [HttpPost]
        public ActionResult Edit(CategoryViewModel model)
        {
            var finder = db.SubCategories.Find(model.Id);
            finder.Name = model.Name;
            var supportCat = db.Categories.Find(model.CategoryId);
            finder.Category = supportCat;

            foreach (var item in model.Translations)
            {
                var curr = finder.Translations.SingleOrDefault(x => x.languageId == item.languageId);
                if (curr != null)
                {
                    curr.Name = item.Name;
                    curr.Description = item.Description;
                }

                else
                    finder.Translations.Add(new SubCategoryTranslation() { languageId = item.languageId, Name = item.Name, Description = item.Description });

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
            ViewBag.categories = db.Categories.ToList();
            CategoryViewModel cvm = new CategoryViewModel() { Id = finder.Id, Translations = new List<CategoryTranslationViewModel>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new CategoryTranslationViewModel() { languageId = item.languageId, Name = item.Name });
            return PartialView(cvm);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var finder = db.SubCategories.Find(id);
            db.SubCategories.Remove(finder);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}