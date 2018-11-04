using AdminPanel.Models.ViewModel;
using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    [Authorize(Roles = "Superadmin,Administrator")]
    public class ProductTypesController : BaseController
    {
        public ActionResult IndexStyles(int page = 1, string search = "")
        {
            int count = 0, pagesize = 15, take = pagesize, skip = (page - 1) * pagesize;


            var data = db.Styles
                .Where(x => string.IsNullOrEmpty(search) || x.Translations.Any(t => t.Name.ToLower().Contains(search.ToLower().Trim())));
            count = data.Count();
            data = data.OrderBy(x => x.insertDate).Skip(skip).Take(take);


            int maxpage = count % pagesize != 0 ? (count / pagesize) + 1 : (count / pagesize);
            ViewBag.page = page; ViewBag.maxpage = maxpage; ViewBag.search = search;

            return View(data.ToList());
        }
        public ActionResult IndexMediums(int page = 1, string search = "")
        {
            int count = 0, pagesize = 15, take = pagesize, skip = (page - 1) * pagesize;

            var data = db.Mediums
                 .Where(x => string.IsNullOrEmpty(search) || x.Translations.Any(t => t.Name.ToLower().Contains(search.ToLower().Trim())));
            count = data.Count();
            data = data.OrderBy(x => x.insertDate).Skip(skip).Take(take);


            int maxpage = count % pagesize != 0 ? (count / pagesize) + 1 : (count / pagesize);
            ViewBag.page = page; ViewBag.maxpage = maxpage; ViewBag.search = search;

            return View(data.ToList());
        }
        public ActionResult IndexSubjects(int page = 1, string search = "")
        {
            int count = 0, pagesize = 15, take = pagesize, skip = (page - 1) * pagesize;

            var data = db.Subjects
                   .Where(x => string.IsNullOrEmpty(search) || x.Translations.Any(t => t.Name.ToLower().Contains(search.ToLower().Trim())));
            count = data.Count();
            data = data.OrderBy(x => x.insertDate).Skip(skip).Take(take);


            int maxpage = count % pagesize != 0 ? (count / pagesize) + 1 : (count / pagesize);
            ViewBag.page = page; ViewBag.maxpage = maxpage; ViewBag.search = search;

            return View(data.ToList());
        }
        public ActionResult IndexMaterials(int page = 1, string search = "")
        {
            int count = 0, pagesize = 15, take = pagesize, skip = (page - 1) * pagesize;

            var data = db.Materials
                   .Where(x => string.IsNullOrEmpty(search) || x.Translations.Any(t => t.Name.ToLower().Contains(search.ToLower().Trim())));
            count = data.Count();
            data = data.OrderBy(x => x.insertDate).Skip(skip).Take(take);


            int maxpage = count % pagesize != 0 ? (count / pagesize) + 1 : (count / pagesize);
            ViewBag.page = page; ViewBag.maxpage = maxpage; ViewBag.search = search;

            return View(data.ToList());
        }


        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult addStyle()
        {
            ViewBag.language = db.Languages.ToList();
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addStyle(ProductTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.language = db.Languages.ToList();
                return PartialView(model);
            }


            Style newmodel = new Style() { AddedByAdmin = true };
            newmodel.Translations = new List<StyleTranslation>();
            foreach (var item in model.Translations)
                newmodel.Translations.Add(new StyleTranslation() { languageId = item.languageId, Name = item.Name });
            db.Styles.Add(newmodel);


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
        public ActionResult editStyle(int id)
        {
            var finder = db.Styles.Find(id);
            ViewBag.language = db.Languages.ToList();
            ProductTypeViewModel cvm = new ProductTypeViewModel() { Id = finder.Id, AddedByAdmin = finder.AddedByAdmin, Translations = new List<ProductTypeTranslationViewModel>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new ProductTypeTranslationViewModel() { languageId = item.languageId, Name = item.Name });
            return PartialView(cvm);
        }
        [HttpPost]
        public ActionResult editStyle(ProductTypeViewModel model)
        {
            var finder = db.Styles.Find(model.Id);

            foreach (var item in model.Translations)
            {
                var curr = finder.Translations.SingleOrDefault(x => x.languageId == item.languageId);
                if (curr != null)
                    curr.Name = item.Name;
                else
                    finder.Translations.Add(new StyleTranslation() { languageId = item.languageId, Name = item.Name });
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
            ProductTypeViewModel cvm = new ProductTypeViewModel() { Id = finder.Id, AddedByAdmin = finder.AddedByAdmin, Translations = new List<ProductTypeTranslationViewModel>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new ProductTypeTranslationViewModel() { languageId = item.languageId, Name = item.Name });
            return PartialView(cvm);
        }
        [HttpGet]
        public ActionResult DeleteStyle(int id)
        {
            var finder = db.Styles.Find(id);
            db.Styles.Remove(finder);
            db.SaveChanges();
            return RedirectToAction("IndexStyles");
        }


        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult addMediums()
        {
            ViewBag.language = db.Languages.ToList();
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addMediums(ProductTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.language = db.Languages.ToList();
                return PartialView(model);
            }


            Medium newmodel = new Medium() { AddedByAdmin = true };
            newmodel.Translations = new List<MediumTranslation>();
            foreach (var item in model.Translations)
                newmodel.Translations.Add(new MediumTranslation() { languageId = item.languageId, Name = item.Name });
            db.Mediums.Add(newmodel);

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
        public ActionResult editMediums(int id)
        {
            var finder = db.Mediums.Find(id);
            ViewBag.language = db.Languages.ToList();
            ProductTypeViewModel cvm = new ProductTypeViewModel() { Id = finder.Id, AddedByAdmin = finder.AddedByAdmin, Translations = new List<ProductTypeTranslationViewModel>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new ProductTypeTranslationViewModel() { languageId = item.languageId, Name = item.Name });
            return PartialView(cvm);
        }
        [HttpPost]
        public ActionResult editMediums(ProductTypeViewModel model)
        {
            var finder = db.Mediums.Find(model.Id);

            foreach (var item in model.Translations)
            {
                var curr = finder.Translations.SingleOrDefault(x => x.languageId == item.languageId);
                if (curr != null)
                    curr.Name = item.Name;
                else
                    finder.Translations.Add(new MediumTranslation() { languageId = item.languageId, Name = item.Name });
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
            ProductTypeViewModel cvm = new ProductTypeViewModel() { Id = finder.Id, AddedByAdmin = finder.AddedByAdmin, Translations = new List<ProductTypeTranslationViewModel>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new ProductTypeTranslationViewModel() { languageId = item.languageId, Name = item.Name });
            return PartialView(cvm);
        }
        [HttpGet]
        public ActionResult DeleteMediums(int id)
        {
            var finder = db.Mediums.Find(id);
            db.Mediums.Remove(finder);
            db.SaveChanges();
            return RedirectToAction("IndexMediums");
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult addSubject()
        {
            ViewBag.language = db.Languages.ToList();
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addSubject(ProductTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.language = db.Languages.ToList();
                return PartialView(model);
            }


            Subject newmodel = new Subject() { };
            newmodel.Translations = new List<SubjectTranslation>();
            foreach (var item in model.Translations)
                newmodel.Translations.Add(new SubjectTranslation() { languageId = item.languageId, Name = item.Name });
            db.Subjects.Add(newmodel);


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
        public ActionResult editSubject(int id)
        {
            var finder = db.Subjects.Find(id);
            ViewBag.language = db.Languages.ToList();
            ProductTypeViewModel cvm = new ProductTypeViewModel() { Id = finder.Id, Translations = new List<ProductTypeTranslationViewModel>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new ProductTypeTranslationViewModel() { languageId = item.languageId, Name = item.Name });
            return PartialView(cvm);
        }
        [HttpPost]
        public ActionResult editSubject(ProductTypeViewModel model)
        {
            var finder = db.Subjects.Find(model.Id);

            foreach (var item in model.Translations)
            {
                var curr = finder.Translations.SingleOrDefault(x => x.languageId == item.languageId);
                if (curr != null)
                    curr.Name = item.Name;
                else
                    finder.Translations.Add(new SubjectTranslation() { languageId = item.languageId, Name = item.Name });
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
            ProductTypeViewModel cvm = new ProductTypeViewModel() { Id = finder.Id, Translations = new List<ProductTypeTranslationViewModel>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new ProductTypeTranslationViewModel() { languageId = item.languageId, Name = item.Name });
            return PartialView(cvm);
        }
        [HttpGet]
        public ActionResult DeleteSubject(int id)
        {
            var finder = db.Subjects.Find(id);
            db.Subjects.Remove(finder);
            db.SaveChanges();
            return RedirectToAction("IndexSubjects");
        }


        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult addMaterial()
        {
            ViewBag.language = db.Languages.ToList();
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addMaterial(ProductTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.language = db.Languages.ToList();
                return PartialView(model);
            }


            Material newmodel = new Material() { AddedByAdmin = true };
            newmodel.Translations = new List<MaterialTranslation>();
            foreach (var item in model.Translations)
                newmodel.Translations.Add(new MaterialTranslation() { languageId = item.languageId, Name = item.Name });
            db.Materials.Add(newmodel);


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
            var finder = db.Materials.Find(id);
            ViewBag.language = db.Languages.ToList();
            ProductTypeViewModel cvm = new ProductTypeViewModel() { Id = finder.Id, AddedByAdmin = finder.AddedByAdmin, Translations = new List<ProductTypeTranslationViewModel>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new ProductTypeTranslationViewModel() { languageId = item.languageId, Name = item.Name });
            return PartialView(cvm);
        }
        [HttpPost]
        public ActionResult editMaterial(ProductTypeViewModel model)
        {
            var finder = db.Materials.Find(model.Id);

            foreach (var item in model.Translations)
            {
                var curr = finder.Translations.SingleOrDefault(x => x.languageId == item.languageId);
                if (curr != null)
                    curr.Name = item.Name;
                else
                    finder.Translations.Add(new MaterialTranslation() { languageId = item.languageId, Name = item.Name });
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
            ProductTypeViewModel cvm = new ProductTypeViewModel() { Id = finder.Id, AddedByAdmin = finder.AddedByAdmin, Translations = new List<ProductTypeTranslationViewModel>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new ProductTypeTranslationViewModel() { languageId = item.languageId, Name = item.Name });
            return PartialView(cvm);
        }
        [HttpGet]
        public ActionResult DeleteMaterial(int id)
        {
            var finder = db.Materials.Find(id);
            db.Materials.Remove(finder);
            db.SaveChanges();
            return RedirectToAction("IndexMaterials");
        }

    }
}