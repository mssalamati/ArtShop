using Blog.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class SearchController : BaseController
    {
        // GET: Search
        public ActionResult Index(string Keyword)
        {
            string currentCultureName = CultureInfo.CurrentCulture.Name.Substring(0, 2);
            var posts = db.Posts.Where(a => a.Translations.FirstOrDefault(x => x.languageId == currentCultureName).Title.Contains(Keyword)).OrderByDescending(a => a.PostedOn).ToList();
            ViewBag.keyword = Keyword;
            return View(posts);

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