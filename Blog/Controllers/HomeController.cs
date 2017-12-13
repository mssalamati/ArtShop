using Blog.Areas.Admin.Controllers;
using Blog.Extentions;
using SimpleMvcSitemap;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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


            //culture = CultureHelper.GetImplementedCulture(culture);
            //HttpCookie cookie = Request.Cookies["_culture"];
            //if (cookie != null)
            //    cookie.Value = culture;   // update cookie value
            //else
            //{
            //    cookie = new HttpCookie("_culture");
            //    cookie.Value = culture;
            //    cookie.Expires = DateTime.Now.AddYears(1);
            //}
            //Response.Cookies.Add(cookie);
            string url = this.Request.UrlReferrer.AbsolutePath + this.Request.UrlReferrer.Query ?? "";

            if (url.Contains("en-us") || url.Contains("fa"))
            {
                Regex re = new Regex("^/\\w{2,3}(-\\w{2})?");
                url = re.Replace(url, "/" + culture.ToLower());
            }
         

            culture = CultureHelper.GetImplementedCulture(culture);
            RouteData.Values["culture"] = culture;  // set culture


            return Redirect(url);
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
        [Route("robots.txt", Name = "GetRobotsText"), OutputCache(Duration = 86400)]
        public ContentResult RobotsText()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("user-agent: *");
            stringBuilder.AppendLine("disallow: /error/");
            stringBuilder.AppendLine("Disallow: /resources");
            stringBuilder.AppendLine("Disallow: /Account/");
            stringBuilder.AppendLine("Disallow: /upload/");
            stringBuilder.AppendLine("allow: /fa/");
            stringBuilder.Append("sitemap: ");
            stringBuilder.AppendLine(this.Url.RouteUrl("GetSitemapXml", null, this.Request.Url.Scheme).TrimEnd('/'));
            stringBuilder.AppendLine(this.Url.RouteUrl("GetSitemapfaXml", null, this.Request.Url.Scheme).TrimEnd('/'));

            return this.Content(stringBuilder.ToString(), "text/plain", Encoding.UTF8);
        }

        [Route("sitemap.xml", Name = "GetSitemapXml"), OutputCache(Duration = 86400)]
        public ActionResult SitemapXml()
        {

            List<SitemapNode> nodes = new List<SitemapNode>
        {
            new SitemapNode(Url.Action("Index", "Home", new { Culture ="en-us" }))
        {
            ChangeFrequency = ChangeFrequency.Weekly,
            LastModificationDate = DateTime.UtcNow,
            Priority = 0.8M
        }

        };

            foreach (var item in db.Categories.ToList())
            {
                nodes.Add(new SitemapNode(@Url.Action("Index", "Category", new { id = @item.GenerateSlug(), Culture = "en-us" }))
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    LastModificationDate = DateTime.UtcNow,
                    Priority = 0.8M
                });
            }


            foreach (var item in db.SubCategories.ToList())
            {
                nodes.Add(new SitemapNode(@Url.Action("SubCategory", "Category", new { id = @item.GenerateSlug(), Culture = "en-us" }))
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    LastModificationDate = DateTime.UtcNow,
                    Priority = 0.8M
                });
            }

            foreach (var item in db.Posts.ToList())
            {
                nodes.Add(new SitemapNode(@Url.Action(@item.Category.Name, "Post", new { id = @item.GenerateSlug(), Culture = "en-us" }))
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    LastModificationDate = DateTime.UtcNow,
                    Priority = 0.8M
                });
            }

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));

        }

        [Route("sitemapfa.xml", Name = "GetSitemapfaXml"), OutputCache(Duration = 86400)]
        public ActionResult SitemapfaXml()
        {

            List<SitemapNode> nodes = new List<SitemapNode>
        {
            new SitemapNode(Url.Action("Index", "Home", new { Culture ="fa" }))
        {
            ChangeFrequency = ChangeFrequency.Weekly,
            LastModificationDate = DateTime.UtcNow,
            Priority = 0.8M
        }

        };

            foreach (var item in db.Categories.ToList())
            {
                nodes.Add(new SitemapNode(@Url.Action("Index", "Category", new { id = @item.GenerateSlug(), Culture = "fa" }))
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    LastModificationDate = DateTime.UtcNow,
                    Priority = 0.8M
                });
            }


            foreach (var item in db.SubCategories.ToList())
            {
                nodes.Add(new SitemapNode(@Url.Action("SubCategory", "Category", new { id = @item.GenerateSlug(), Culture = "fa" }))
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    LastModificationDate = DateTime.UtcNow,
                    Priority = 0.8M
                });
            }

            foreach (var item in db.Posts.ToList())
            {
                nodes.Add(new SitemapNode(@Url.Action(@item.Category.Name, "Post", new { id = @item.GenerateSlug(), Culture = "fa" }))
                {
                    ChangeFrequency = ChangeFrequency.Weekly,
                    LastModificationDate = DateTime.UtcNow,
                    Priority = 0.8M
                });
            }

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }
    }
}