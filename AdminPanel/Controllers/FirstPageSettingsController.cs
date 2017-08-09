using AdminPanel.Models.ViewModel;
using DataLayer;
using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace AdminPanel.Controllers
{
    [Authorize(Users = "superadmin")]
    public class FirstPageSettingsController : Controller
    {
        protected ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Header()
        {
            return View(db.NavigationCategories.Include("category"));
        }
        [HttpPost]
        public ActionResult Header(int id)
        {
            if (db.NavigationCategories.Any(x => x.categoryId == id))
            {
                ViewBag.error = "این دسته بندی تکراری است";
                return View(db.NavigationCategories.Include("category"));
            }

            db.NavigationCategories.Add(new NavigationCategory()
            {
                categoryId = id
            });
            ViewBag.success = "ثبت شد";
            db.SaveChanges();
            return View(db.NavigationCategories.Include("category"));
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
            return RedirectToAction("Header");
        }


        public ActionResult slider()
        {
            return View(db.sliderImages.Include("Translations"));
        }
        public ActionResult Addslider()
        {
            ViewBag.language = db.Languages.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Addslider(sliderImage slider)
        {
            var file = Request.Files[0];
            string tempFolderName = "Upload/Slider_Images";
            var result = ImageHelper.Saveimage(Server, file, tempFolderName, ImageHelper.saveImageMode.wide);
            if (!result.ResultStatus)
            {
                ViewBag.language = db.Languages.ToList();
                ModelState.AddModelError(string.Empty, result.Error);
                ViewBag.SiteParams = db.SiteParams.ToList();
                return View();
            }
            var site = db.sliderImages.Add(new sliderImage()
            {
                path = result.FullPath,
                ButtonURL = slider.ButtonURL,
                TextColor = slider.TextColor,
                Translations = slider.Translations
            });
            db.SaveChanges();
            return RedirectToAction("slider", db.sliderImages.Include("Translations"));
        }
        public ActionResult DeleteSlider(int id)
        {
            var finder = db.sliderImages.Find(id);
            db.sliderImages.Remove(finder);
            db.SaveChanges();
            return RedirectToAction("slider", db.sliderImages.Include("Translations"));
        }


        public ActionResult maincontent()
        {
            return View(db.FirstPageSections);
        }
        public ActionResult Addmaincontent()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Addmaincontent(FirstPageSection model)
        {
            db.FirstPageSections.Add(model);
            db.SaveChanges();
            return PartialView("_successWindow");
        }
        public ActionResult Deletesection(int id)
        {
            var finder = db.FirstPageSections.Find(id);
            db.FirstPageSections.Remove(finder);
            db.SaveChanges();
            return RedirectToAction("maincontent", db.FirstPageSections);
        }


        public ActionResult footers()
        {
            return View(db.footerCells.Include("Translations"));
        }
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult addfooter()
        {
            ViewBag.language = db.Languages.ToList();
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addfooter(footerCell model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.language = db.Languages.ToList();
                return PartialView(model);
            }

            footerCell newmodel = new footerCell() {  };
            newmodel.Translations = new List<footerCellTranslation>();
            foreach (var item in model.Translations)
                newmodel.Translations.Add(new footerCellTranslation() { languageId = item.languageId, Header = item.Header });
            db.footerCells.Add(newmodel);

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
        public ActionResult deletefooter(int id)
        {
            var finder = db.footerCells.Find(id);
            db.footerCells.Remove(finder);
            db.SaveChanges();
            return RedirectToAction("footers", db.footerCells.Include("Translations"));
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
                    finder.Translations.Add(new SiteParamTranslation { languageId = item.languageId, Value = item.Value });
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


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}