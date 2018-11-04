using AdminPanel.Models.ViewModel;
using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace AdminPanel.Controllers
{
    [Authorize(Roles = "Superadmin,Administrator")]
    public class CategoriesController : BaseController
    {
        // GET: Categories
        public ActionResult Index()
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

            string tempFolderName = "Upload/Category_Images";
            var result = ImageHelper.Saveimage(Server, model.Image, tempFolderName, ImageHelper.saveImageMode.Squre);
            if (!result.ResultStatus)
            {
                ViewBag.language = db.Languages.ToList();
                ModelState.AddModelError(string.Empty, result.Error);
                return PartialView(model);
            }

            Photo p = new Photo() { Path = result.FullPath, width = result.Width, Height = result.Height };
            Category c = new Category() { photo = p };
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
            CategoryViewModel cvm = new CategoryViewModel() { Id = finder.Id, ImagePath = finder.photo.Path, Translations = new List<CategoryTranslationViewModel>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new CategoryTranslationViewModel() { languageId = item.languageId, Name = item.Name });

            return PartialView(cvm);
        }

        [HttpPost]
        public ActionResult Edit(CategoryViewModel model)
        {
            var finder = db.Categories.Find(model.Id);

            if (model.Image != null)
            {
                string tempFolderName = "Upload/Category_Images";
                var result = ImageHelper.Saveimage(Server, model.Image, tempFolderName, ImageHelper.saveImageMode.Squre);
                if (!result.ResultStatus)
                {
                    ViewBag.language = db.Languages.ToList();
                    ModelState.AddModelError(string.Empty, result.Error);
                    return PartialView(model);
                }
                else
                    finder.photo.Path = result.FullPath;
            }

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
            CategoryViewModel cvm = new CategoryViewModel() { Id = finder.Id, ImagePath = finder.photo.Path, Translations = new List<CategoryTranslationViewModel>() };
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