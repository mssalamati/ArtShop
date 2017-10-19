using Blog.Areas.Admin.Controllers;
using Blog.Extentions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(int page = 1)
        {
            string currentCultureName = CultureInfo.CurrentCulture.Name.Substring(0, 2);
            ViewBag.ShowCase = db.Posts.FirstOrDefault(a => a.postType == Objects.PostType.ShowCase);
            var post = db.Posts.Where(a => a.Category.Name.ToLower() != "galleries" && a.postType == Objects.PostType.Sqr && a.Translations.Any(x => x.languageId == currentCultureName && (x.Description.Length != 0 && x.Description != null))).OrderByDescending(a => a.PostedOn).Take(21).ToList();
            return View(post);
        }

        public ActionResult More(int page = 1)
        {
            string currentCultureName = CultureInfo.CurrentCulture.Name.Substring(0, 2);
            var post = db.Posts.Where(a => a.Category.Name.ToLower() != "galleries" && a.postType == Objects.PostType.Sqr && a.Translations.Any(x => x.languageId == currentCultureName && (x.Description.Length != 0 && x.Description != null))).OrderByDescending(a => a.PostedOn).Skip((page - 1) * 21).Take(21).ToList();
            return PartialView(post);
        }

        public ActionResult Header()
        {
            var Header = db.NavigationCategories.OrderBy(a => a.priority).Select(x => x.category).ToList();
            return PartialView("_Header", Header);
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
            return RedirectPermanent(url);
        }

        public ActionResult AddSubscriber(string email)
        {
            var client = new RestSharp.RestClient("https://api.mailerlite.com/api/v2/groups/8129891/subscribers");
            var request = new RestSharp.RestRequest(RestSharp.Method.POST);
            request.AddHeader("x-mailerlite-apikey", "0e0ba56cc888feb4f4573cfe0a5f497c");
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"email\":\"" + email + "\", \"name\": \" \", \"fields\": {\"company\": \"Artiscovery\"}}", RestSharp.ParameterType.RequestBody);
            RestSharp.IRestResponse response = client.Execute(request);

            return Content("done");
        }
    }
}