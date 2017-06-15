using AdminPanel.Models.ViewModel;
using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class FirstPageSettingsController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.SiteObjectParams = db.SiteObjectParams.FirstOrDefault();
            ViewBag.SiteParams = db.SiteParams.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AddCategoryToHeader(int id)
        {
            var obj = db.SiteObjectParams.Single();
            obj.Navigations.Add(new DataLayer.Enitities.NavigationCategory()
            {
                categoryId = id
            });
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EditCatHeader(int id)
        {
            var finder = db.NavigationCategories.Find(id);
            return PartialView(finder);
        }

        [HttpPost]
        public ActionResult EditCatHeader(NavigationCategoryViewModel model)
        {
            var finder = db.NavigationCategories.Find(model.Id);

            try
            {
                finder.FavStyles.Clear();
                finder.FavMediums.Clear();
                finder.FavSubjects.Clear();

                if (model.FavStyles == null) model.FavStyles = new List<int>();
                if (model.FavMediums == null) model.FavMediums = new List<int>();
                if (model.FavSubjects == null) model.FavSubjects = new List<int>();

                var FavStyles = model.FavStyles.Select(x => new NavigationCategoryFavStyle() { styleId = x });
                var FavMediums = model.FavMediums.Select(x => new NavigationCategoryFavMedium() { mediumId = x });
                var FavSubjects = model.FavSubjects.Select(x => new NavigationCategorySubject() { subjectId = x });

                foreach (var item in FavStyles)
                    finder.FavStyles.Add(item);

                foreach (var item in FavMediums)
                    finder.FavMediums.Add(item);

                foreach (var item in FavSubjects)
                    finder.FavSubjects.Add(item);

                db.SaveChanges();
                ViewBag.alert = "با موفقیت انجام شد";
            }
            catch
            {
                ViewBag.alert = "خطا در ورود اطلاعات";
            }

            return PartialView(finder);
        }

        public ActionResult DeleteCatHeader(int id)
        {
            var finder = db.NavigationCategories.Find(id);
            db.NavigationCategories.Remove(finder);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult editParams(string id)
        {
            var finder = db.SiteParams.Find(id);
            ViewBag.language = db.Languages.ToList();

            return PartialView(finder);
        }

        [HttpPost]
        public ActionResult editParams(SiteParam model)
        {
            var finder = db.SiteParams.Find(model.Name);

            foreach (var item in model.Translations)
            {
                var curr = finder.Translations.SingleOrDefault(x => x.languageId == item.languageId);
                if (curr != null)
                    curr.Value = item.Value;
                else
                    finder.Translations.Add(new SiteParamTranslation  { languageId = item.languageId, Value = item.Value });
            }

            try
            {
                db.SaveChanges();
                ViewBag.alert = "با موفقیت انجام شد";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.ToString());
            }

            ViewBag.language = db.Languages.ToList();
            return PartialView(finder);
        }
    }
}