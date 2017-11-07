using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace AdminPanel.Controllers
{
    public class MobileHomePageController : BaseController
    {
        public ActionResult Index()
        {
            var data = db.MobileHomePageItems;
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
        public ActionResult add(MobileHomePage model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.language = db.Languages.ToList();
                return PartialView(model);
            }
            string tempFolderName = "Upload/Mobile_Home_Images";
            var result = ImageHelper.Saveimage(Server, model.Image, tempFolderName, ImageHelper.saveImageMode.Squre);
            if (!result.ResultStatus)
            {
                ViewBag.language = db.Languages.ToList();
                ModelState.AddModelError(string.Empty, result.Error);
                return PartialView(model);
            }

            MobileHomePage newmodel = new MobileHomePage() { Title = model.Title, PhotoPath = result.FullPath };
            newmodel.Translations = new List<MobileHomePageTranslation>();
            foreach (var item in model.Translations)
                newmodel.Translations.Add(new MobileHomePageTranslation() { languageId = item.languageId, Title = item.Title });
            db.MobileHomePages.Add(newmodel);
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
            var finder = db.MobileHomePages.Find(id);
            ViewBag.language = db.Languages.ToList();
            return PartialView(finder);
        }

        [HttpPost]
        public ActionResult edit(MobileHomePage model)
        {
            var finder = db.MobileHomePages.Find(model.Id);

            if (model.Image != null)
            {
                string tempFolderName = "Upload/Mobile_Home_Images";
                var result = ImageHelper.Saveimage(Server, model.Image, tempFolderName, ImageHelper.saveImageMode.Squre);
                if (!result.ResultStatus)
                {
                    ViewBag.language = db.Languages.ToList();
                    ModelState.AddModelError(string.Empty, result.Error);
                    return PartialView(model);
                }
                else
                    finder.PhotoPath = result.FullPath;
            }

            foreach (var item in model.Translations)
            {
                var curr = finder.Translations.SingleOrDefault(x => x.languageId == item.languageId);
                if (curr != null)
                    curr.Title = item.Title;
                else
                    finder.Translations.Add(new MobileHomePageTranslation() { languageId = item.languageId, Title = item.Title });
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
            return PartialView(model);
        }

        public ActionResult Products(int id)
        {
            var finder = db.MobileHomePages.Find(id);
            return PartialView(finder);
        }
        public ActionResult AddProduct(int homeId, int productId)
        {
            var finder = db.MobileHomePages.Find(homeId);
            finder.Items.Add(new MobileHomePageItem()
            {
                ProductId = productId
            });
            db.SaveChanges();
            return PartialView();
        }
        public ActionResult RemoveProduct(int id)
        {
            var finder = db.MobileHomePageItems.Find(id);
            db.MobileHomePageItems.Remove(finder);
            db.SaveChanges();
            return PartialView();
        }
        public ActionResult getProduct(int id)
        {
            var finder = db.Products.Find(id);
            return Json(new { finder.Title, id = finder.Id }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var finder = db.MobileHomePages.Find(id);
            db.MobileHomePages.Remove(finder);
            db.SaveChanges();
            return RedirectToAction("index");
        }
    }
}