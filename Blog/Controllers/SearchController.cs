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
    }
}