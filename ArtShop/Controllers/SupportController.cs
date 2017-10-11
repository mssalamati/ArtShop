using ArtShop.Helper;
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
            ViewBag.keyword = keyword;
            var result = db.Articles.Where(a => a.Title.Contains(keyword)).ToList();
            return View(result);
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

        public ActionResult SetCulture(string culture)
        {
            HttpCookie popupCookie = Request.Cookies["isShown"];
            if (popupCookie != null)
                popupCookie.Value = "true";   // update cookie value
            else
            {
                popupCookie = new HttpCookie("isShown");
                popupCookie.Value = "true";
                popupCookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(popupCookie);


            culture = CultureHelper.GetImplementedCulture(culture);
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            string url = this.Request.UrlReferrer.AbsolutePath + this.Request.UrlReferrer.Query ?? "";
            return Redirect(url);
        }
    }
}