using AdminPanel.Models.ViewModel;
using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class CountriesController : BaseController
    {
        public ActionResult Index(int page = 1, string search = "")
        {
            int count = 0, pagesize = 15, take = pagesize, skip = (page - 1) * pagesize;

            var data = db.Countries
                 .Where(x => string.IsNullOrEmpty(search) || x.Translations.Any(t => t.Name.Contains(search)))
                 .OrderBy(x => x.Translations.FirstOrDefault().Name)
                 .Skip(skip).Take(take);
            count = db.Countries.Count();

            int maxpage = count % pagesize != 0 ? (count / pagesize) + 1 : (count / pagesize);
            ViewBag.page = page; ViewBag.maxpage = maxpage; ViewBag.search = search;

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
        public ActionResult add(CountryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.language = db.Languages.ToList();
                return PartialView(model);
            }

            Country newmodel = new Country() { Code = model.code, region = model.region };
            newmodel.Translations = new List<CountryTranslation>();
            foreach (var item in model.Translations)
                newmodel.Translations.Add(new CountryTranslation() { languageId = item.languageId, Name = item.Name });
            db.Countries.Add(newmodel);

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
            var finder = db.Countries.Find(id);
            ViewBag.language = db.Languages.ToList();
            CountryViewModel cvm = new CountryViewModel() { Id = finder.Id, code = finder.Code, region = finder.region, Translations = new List<CountryTranslationViewModel>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new CountryTranslationViewModel() { languageId = item.languageId, Name = item.Name });
            return PartialView(cvm);
        }

        [HttpPost]
        public ActionResult edit(CountryViewModel model)
        {
            var finder = db.Countries.Find(model.Id);

            finder.Code = model.code;
            finder.region = model.region;
            foreach (var item in model.Translations)
            {
                var curr = finder.Translations.SingleOrDefault(x => x.languageId == item.languageId);
   
                if (curr != null)
                    curr.Name = item.Name;
                else
                    finder.Translations.Add(new CountryTranslation() { languageId = item.languageId, Name = item.Name });
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
            CountryViewModel cvm = new CountryViewModel() { Id = finder.Id, code = finder.Code, Translations = new List<CountryTranslationViewModel>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new CountryTranslationViewModel() { languageId = item.languageId, Name = item.Name });
            return PartialView(cvm);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var finder = db.Countries.Find(id);
            db.Countries.Remove(finder);            
            db.Entry(finder).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("index");
        }

    }
}