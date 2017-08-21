using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Extentions;
namespace ArtShop.Controllers
{
    public class PagesController : BaseController
    {
        public ActionResult About()
        {
            var page = db.SitePages.Include("Translations").SingleOrDefault(x => x.DefaultTitle == "About");
            return View(page.Current());
        }
    }
}