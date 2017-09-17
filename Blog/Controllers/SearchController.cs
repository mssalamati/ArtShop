using Blog.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
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
            var posts = db.Posts.Where(x => x.Title.Contains(Keyword)).OrderByDescending(a => a.PostedOn).ToList();
            ViewBag.keyword = Keyword;
            return View(posts);

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