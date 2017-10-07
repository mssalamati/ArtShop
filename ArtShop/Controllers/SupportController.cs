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
            var data = db.SupportCategories.Where(a=>a.categoryType == DataLayer.Enitities.CategoryType.Artist).ToList();
            return View(data);
        }

        public ActionResult Header()
        {
            
            return PartialView("_Header");
        }
    }
}