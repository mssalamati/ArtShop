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
    public class PriceListController : BaseController
    {
        public ActionResult Index()
        {
            var data = db.Pricethresholds;
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
        public ActionResult add(PriceListViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.language = db.Languages.ToList();
                return PartialView(model);
            }

            Pricethreshold newmodel = new Pricethreshold() { min = model.min, max = model.max };
            newmodel.Translations = new List<PricethresholdTranslation>();
            foreach (var item in model.Translations)
                newmodel.Translations.Add(new PricethresholdTranslation() { languageId = item.languageId, Name = item.Name });
            db.Pricethresholds.Add(newmodel);

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
            var finder = db.Pricethresholds.Find(id);
            ViewBag.language = db.Languages.ToList();
            PriceListViewModel cvm = new PriceListViewModel() { Id = finder.Id, min = finder.min, max = finder.max, Translations = new List<PriceListTranslationViewModel>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new PriceListTranslationViewModel() { languageId = item.languageId, Name = item.Name });
            return PartialView(cvm);
        }

        [HttpPost]
        public ActionResult edit(PriceListViewModel model)
        {
            var finder = db.Pricethresholds.Find(model.Id);
            finder.max = model.max;
            finder.min = model.min;
            foreach (var item in model.Translations)
            {
                var curr = finder.Translations.SingleOrDefault(x => x.languageId == item.languageId);
                if (curr != null)
                    curr.Name = item.Name;
                else
                    finder.Translations.Add(new PricethresholdTranslation() { languageId = item.languageId, Name = item.Name });
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
            PriceListViewModel cvm = new PriceListViewModel() { Id = finder.Id, min = finder.min, max = finder.max, Translations = new List<PriceListTranslationViewModel>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new PriceListTranslationViewModel() { languageId = item.languageId, Name = item.Name });
            return PartialView(cvm);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var finder = db.Pricethresholds.Find(id);
            db.Pricethresholds.Remove(finder);
            db.SaveChanges();
            return RedirectToAction("index");
        }

    }
}