using Blog.Areas.Admin.Controllers;
using Blog.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(int page = 1)
        {
            ViewBag.ShowCase = db.Posts.FirstOrDefault(a => a.postType == Objects.PostType.ShowCase);
            var post = db.Posts.Where(a => a.Category.Name.ToLower() != "galleries").OrderByDescending(a => a.PostedOn).Take(19).ToList();
            return View(post);
        }

        public ActionResult More(int page = 1)
        {
            var post = db.Posts.Where(a => a.Category.Name.ToLower() != "galleries").OrderByDescending(a => a.PostedOn).Skip((page - 1) * 19).Take(19).ToList();
            return PartialView(post);
        }

        public ActionResult Header()
        {
            var Header = db.NavigationCategories.OrderBy(a => a.priority).Select(x => x.category).ToList();
            return PartialView("_Header", Header);
        }

        public ActionResult SetCulture(string culture)
        {
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

        public ActionResult AddSubscriber(string email)
        {
            var client = new RestSharp.RestClient("https://api.mailerlite.com/api/v2/groups/7737389/subscribers");
            var request = new RestSharp.RestRequest(RestSharp.Method.POST);
            request.AddHeader("x-mailerlite-apikey", "0e0ba56cc888feb4f4573cfe0a5f497c");
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"email\":\"" + email + "\", \"name\": \" \", \"fields\": {\"company\": \"Artiscovery\"}}", RestSharp.ParameterType.RequestBody);
            RestSharp.IRestResponse response = client.Execute(request);

            return Content("done");
        }
    }
}