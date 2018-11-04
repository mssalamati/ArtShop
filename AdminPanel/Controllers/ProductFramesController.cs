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
    public class ProductFramesController : BaseController
    {
        //color
        public ActionResult Indexcolor()
        {
            var data = db.ProductFrameColors.OrderBy(x => x.Name);
            return View(data.ToList());
        }
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult addcolor()
        {
            ViewBag.language = db.Languages.ToList();
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addcolor(ProductFrameColor model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.language = db.Languages.ToList();
                return PartialView(model);
            }

            ProductFrameColor newmodel = new ProductFrameColor() { Name = model.Name };
            newmodel.Translations = new List<ProductFrameColorTranslation>();
            foreach (var item in model.Translations)
                newmodel.Translations.Add(new ProductFrameColorTranslation() { languageId = item.languageId, Name = item.Name });
            db.ProductFrameColors.Add(newmodel);

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
        public ActionResult editcolor(int id)
        {
            var finder = db.ProductFrameColors.Find(id);
            ViewBag.language = db.Languages.ToList();
            ProductFrameColor cvm = new ProductFrameColor() { Id = finder.Id, Name = finder.Name, Translations = new List<ProductFrameColorTranslation>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new ProductFrameColorTranslation() { languageId = item.languageId, Name = item.Name });
            return PartialView(cvm);
        }
        [HttpPost]
        public ActionResult editcolor(ProductFrameColor model)
        {
            var finder = db.ProductFrameColors.Find(model.Id);

            finder.Name = model.Name;
            foreach (var item in model.Translations)
            {
                var curr = finder.Translations.SingleOrDefault(x => x.languageId == item.languageId);
                if (curr != null)
                    curr.Name = item.Name;
                else
                    finder.Translations.Add(new ProductFrameColorTranslation() { languageId = item.languageId, Name = item.Name });
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
            ProductFrameColor cvm = new ProductFrameColor() { Id = finder.Id, Name = finder.Name, Translations = new List<ProductFrameColorTranslation>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new ProductFrameColorTranslation() { languageId = item.languageId, Name = item.Name });
            return PartialView(cvm);
        }
        [HttpGet]
        public ActionResult Deletecolor(int id)
        {
            var finder = db.ProductFrameColors.Find(id);
            db.ProductFrameColors.Remove(finder);
            db.SaveChanges();
            return RedirectToAction("Indexcolor");
        }
        //type
        public ActionResult IndexType()
        {
            var data = db.ProductFrameTypes.OrderBy(x => x.Name);
            return View(data.ToList());
        }
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult addType()
        {
            ViewBag.language = db.Languages.ToList();
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addType(ProductFrameType model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.language = db.Languages.ToList();
                return PartialView(model);
            }

            ProductFrameType newmodel = new ProductFrameType() { Name = model.Name };
            newmodel.Translations = new List<ProductFrameTypeTranslation>();
            foreach (var item in model.Translations)
                newmodel.Translations.Add(new ProductFrameTypeTranslation() { languageId = item.languageId, Name = item.Name });
            db.ProductFrameTypes.Add(newmodel);

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
        public ActionResult editType(int id)
        {
            var finder = db.ProductFrameTypes.Find(id);
            ViewBag.language = db.Languages.ToList();
            ProductFrameType cvm = new ProductFrameType() { Id = finder.Id, Name = finder.Name, Translations = new List<ProductFrameTypeTranslation>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new ProductFrameTypeTranslation() { languageId = item.languageId, Name = item.Name });
            return PartialView(cvm);
        }
        [HttpPost]
        public ActionResult editType(ProductFrameColor model)
        {
            var finder = db.ProductFrameTypes.Find(model.Id);

            finder.Name = model.Name;
            foreach (var item in model.Translations)
            {
                var curr = finder.Translations.SingleOrDefault(x => x.languageId == item.languageId);
                if (curr != null)
                    curr.Name = item.Name;
                else
                    finder.Translations.Add(new ProductFrameTypeTranslation() { languageId = item.languageId, Name = item.Name });
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
            ProductFrameType cvm = new ProductFrameType() { Id = finder.Id, Name = finder.Name, Translations = new List<ProductFrameTypeTranslation>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new ProductFrameTypeTranslation() { languageId = item.languageId, Name = item.Name });
            return PartialView(cvm);
        }
        [HttpGet]
        public ActionResult DeleteType(int id)
        {
            var finder = db.ProductFrameTypes.Find(id);
            db.ProductFrameTypes.Remove(finder);
            db.SaveChanges();
            return RedirectToAction("IndexType");
        }
        //material
        public ActionResult IndexMaterial()
        {
            var data = db.ProductFrameMaterials.OrderBy(x => x.Name);
            return View(data.ToList());
        }
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult addMaterial()
        {
            ViewBag.language = db.Languages.ToList();
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addMaterial(ProductFrameMaterial model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.language = db.Languages.ToList();
                return PartialView(model);
            }

            ProductFrameMaterial newmodel = new ProductFrameMaterial() { Name = model.Name };
            newmodel.Translations = new List<ProductFrameMaterialTranslation>();
            foreach (var item in model.Translations)
                newmodel.Translations.Add(new ProductFrameMaterialTranslation() { languageId = item.languageId, Name = item.Name });
            db.ProductFrameMaterials.Add(newmodel);

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
        public ActionResult editMaterial(int id)
        {
            var finder = db.ProductFrameMaterials.Find(id);
            ViewBag.language = db.Languages.ToList();
            ProductFrameMaterial cvm = new ProductFrameMaterial() { Id = finder.Id, Name = finder.Name, Translations = new List<ProductFrameMaterialTranslation>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new ProductFrameMaterialTranslation() { languageId = item.languageId, Name = item.Name });
            return PartialView(cvm);
        }
        [HttpPost]
        public ActionResult editMaterial(ProductFrameMaterial model)
        {
            var finder = db.ProductFrameMaterials.Find(model.Id);

            finder.Name = model.Name;
            foreach (var item in model.Translations)
            {
                var curr = finder.Translations.SingleOrDefault(x => x.languageId == item.languageId);
                if (curr != null)
                    curr.Name = item.Name;
                else
                    finder.Translations.Add(new ProductFrameMaterialTranslation() { languageId = item.languageId, Name = item.Name });
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
            ProductFrameMaterial cvm = new ProductFrameMaterial() { Id = finder.Id, Name = finder.Name, Translations = new List<ProductFrameMaterialTranslation>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new ProductFrameMaterialTranslation() { languageId = item.languageId, Name = item.Name });
            return PartialView(cvm);
        }
        [HttpGet]
        public ActionResult DeleteMaterial(int id)
        {
            var finder = db.ProductFrameMaterials.Find(id);
            db.ProductFrameMaterials.Remove(finder);
            db.SaveChanges();
            return RedirectToAction("IndexMaterial");
        }
    }
}