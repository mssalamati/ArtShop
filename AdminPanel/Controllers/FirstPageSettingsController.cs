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
    }
}