using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtShop.Controllers
{
    public class SupportController : BaseController
    {
        // GET: Support
        public ActionResult Index()
        {
            var data = db.Articles.Where(a=>a.isHandbook == true).ToList();
            return View(data);
        }

        public ActionResult Search(string keyword)
        {

            return View();
        }

        public ActionResult Article(int id)
        {
            var article = db.Articles.Find(id);
            return View(article);
        }

        public ActionResult Category(int id)
        {
            var category = db.SupportCategories.Find(id);
            return View(category);
        }

        public ActionResult SubCategory(int id)
        {
            var subCategory = db.SupportSubCategories.Find(id);
            return View(subCategory);
        }

        public ActionResult Header()
        {
            
            return PartialView("_Header");
        }
    }
}