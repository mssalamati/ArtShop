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
        public ActionResult Terms()
        {
            var page = db.SitePages.Include("Translations").SingleOrDefault(x => x.DefaultTitle == "Terms");
            return View(page.Current());
        }
        public ActionResult PrivacyPolicy()
        {
            var page = db.SitePages.Include("Translations").SingleOrDefault(x => x.DefaultTitle == "Privacy Policy");
            return View(page.Current());
        }
        public ActionResult CopyrightPolicy()
        {
            var page = db.SitePages.Include("Translations").SingleOrDefault(x => x.DefaultTitle == "Copyright Policy");
            return View(page.Current());
        }
        public ActionResult WhySell()
        {
            var page = db.SitePages.Include("Translations").SingleOrDefault(x => x.DefaultTitle == "Why Sell");
            return View(page.Current());
        }
        public ActionResult Contactus()
        {
            var page = db.SitePages.Include("Translations").SingleOrDefault(x => x.DefaultTitle == "Contact us");
            return View(page.Current());
        }
    }
}