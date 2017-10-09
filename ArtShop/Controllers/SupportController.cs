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

        public ActionResult Article(int id)
        {
            var article = db.Articles.Find(id);

            return View(article);
        }

        public ActionResult Categories()
        {


            return View();
        }

        public ActionResult Header()
        {
            
            return PartialView("_Header");
        }
    }
}