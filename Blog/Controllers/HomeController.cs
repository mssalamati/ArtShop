using Blog.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.ShowCase = db.Posts.FirstOrDefault(a => a.postType == Objects.PostType.ShowCase);
            var post = db.Posts.OrderByDescending(a => a.PostedOn).Take(20).ToList();
            return View(post);
        }

        public ActionResult Header()
        {
            var Header = db.NavigationCategories.OrderBy(a => a.priority).Select(x => x.category).ToList();
            return PartialView("_Header",Header);
        }
    }
}