using AdminPanel.Models;
using AdminPanel.Models.ViewModel;
using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    [Authorize(Roles = "Superadmin,Administrator,Manager")]
    public class PrintOptionsController : BaseController
    {
        public ActionResult Index()
        {
            var data = db.PrintMaterials.Include("Translations");
            return View(data.ToList());
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult add()
        {
            ViewBag.language = db.Languages.ToList();
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult add(PrintMaterialViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.language = db.Languages.ToList();
                return PartialView(model);
            }

            PrintMaterial newmodel = new PrintMaterial();
            newmodel.Translations = new List<PrintMaterialTranslation>();
            foreach (var item in model.Translations)
                newmodel.Translations.Add(new PrintMaterialTranslation() { languageId = item.languageId, title = item.title });
            db.PrintMaterials.Add(newmodel);
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

        public ActionResult edit(int id)
        {
            var finder = db.PrintMaterials.Find(id);
            ViewBag.language = db.Languages.ToList();
            PrintMaterialViewModel cvm = new PrintMaterialViewModel() { Id = finder.Id, Translations = new List<PrintMaterialTranslationViewModel>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new PrintMaterialTranslationViewModel() { languageId = item.languageId, title = item.title });
            return PartialView(cvm);
        }

        [HttpPost]
        public ActionResult edit(PrintMaterialViewModel model)
        {
            var finder = db.PrintMaterials.Find(model.Id);
            foreach (var item in model.Translations)
            {
                var curr = finder.Translations.SingleOrDefault(x => x.languageId == item.languageId);
                if (curr != null)
                    curr.title = item.title;
                else
                    finder.Translations.Add(new PrintMaterialTranslation() { languageId = item.languageId, title = item.title });
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
            PrintMaterialViewModel cvm = new PrintMaterialViewModel() { Id = finder.Id, Translations = new List<PrintMaterialTranslationViewModel>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new PrintMaterialTranslationViewModel() { languageId = item.languageId, title = item.title });
            return PartialView(cvm);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var finder = db.PrintMaterials.Find(id);
            db.PrintMaterials.Remove(finder);
            db.SaveChanges();
            return RedirectToAction("index");
        }

        //size
        public ActionResult addSize(int id)
        {
            ViewBag.language = db.Languages.ToList();
            ViewBag.materialid = id;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addSize(PrintMaterialViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.language = db.Languages.ToList();
                ViewBag.materialid = model.parentid;
                return PartialView(model);
            }

            PrintSize newmodel = new PrintSize();
            newmodel.title = model.title;
            newmodel.price = model.price;
            newmodel.printMaterialId = model.parentid;
            newmodel.Translations = new List<PrintSizeTranslation>();
            foreach (var item in model.Translations)
                newmodel.Translations.Add(new PrintSizeTranslation() { languageId = item.languageId, title = item.title });

            db.PrintSizes.Add(newmodel);
            try
            {
                db.SaveChanges();
                return PartialView("_successWindow");
            }
            catch (Exception ex)
            {
                ViewBag.language = db.Languages.ToList();
                ViewBag.materialid = model.parentid;
                ModelState.AddModelError(string.Empty, ex.ToString());
                return PartialView(model);
            }
        }

        [HttpGet]
        public ActionResult DeleteSize(int id)
        {
            var finder = db.PrintSizes.Find(id);
            db.PrintSizes.Remove(finder);
            db.SaveChanges();
            return RedirectToAction("index");
        }

        public ActionResult editSize(int id)
        {
            var finder = db.PrintSizes.Find(id);
            ViewBag.language = db.Languages.ToList();
            PrintMaterialViewModel cvm = new PrintMaterialViewModel()
            {
                Id = finder.Id,
                title = finder.title,
                price = finder.price,
                Width = finder.Width,
                Height = finder.Height,
                Translations = new List<PrintMaterialTranslationViewModel>()
            };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new PrintMaterialTranslationViewModel() { languageId = item.languageId, title = item.title });
            return PartialView(cvm);
        }

        [HttpPost]
        public ActionResult editSize(PrintMaterialViewModel model)
        {
            var finder = db.PrintSizes.Find(model.Id);
            finder.title = model.title;
            finder.price = model.price;
            finder.Width = model.Width;
            finder.Height = model.Height;
            foreach (var item in model.Translations)
            {
                var curr = finder.Translations.SingleOrDefault(x => x.languageId == item.languageId);
                if (curr != null)
                    curr.title = item.title;
                else
                    finder.Translations.Add(new PrintSizeTranslation() { languageId = item.languageId, title = item.title });
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
            PrintMaterialViewModel cvm = new PrintMaterialViewModel()
            {
                Id = finder.Id,
                title = finder.title,
                price = finder.price,
                Translations = new List<PrintMaterialTranslationViewModel>()
            };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new PrintMaterialTranslationViewModel() { languageId = item.languageId, title = item.title });
            return PartialView(cvm);
        }

        //frame
        public ActionResult addFrame(int id)
        {
            ViewBag.language = db.Languages.ToList();
            ViewBag.sizeId = id;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addFrame(PrintMaterialViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.language = db.Languages.ToList();
                ViewBag.sizeId = model.parentid;
                return PartialView(model);
            }

            PrintFrame newmodel = new PrintFrame();
            newmodel.title = model.title;
            newmodel.price = model.price;
            newmodel.color = model.color;
            newmodel.size = model.size;
            newmodel.printSizeId = model.parentid;
            newmodel.Translations = new List<PrintFrameTranslation>();
            foreach (var item in model.Translations)
                newmodel.Translations.Add(new PrintFrameTranslation() { languageId = item.languageId, title = item.title });
            db.PrintFrames.Add(newmodel);
            try
            {
                db.SaveChanges();
                return PartialView("_successWindow");
            }
            catch (Exception ex)
            {
                ViewBag.language = db.Languages.ToList();
                ViewBag.sizeId = model.parentid;
                ModelState.AddModelError(string.Empty, ex.ToString());
                return PartialView(model);
            }
        }

        [HttpGet]
        public ActionResult DeleteFrame(int id)
        {
            var finder = db.PrintFrames.Find(id);
            db.PrintFrames.Remove(finder);
            db.SaveChanges();
            return RedirectToAction("index");
        }

    }
}