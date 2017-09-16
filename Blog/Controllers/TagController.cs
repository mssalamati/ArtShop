using Blog.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class TagController : BaseController
    {
        // GET: Tag
        public ActionResult Index(int id)
        {
            var tags = db.Tags.Find(id);
            var posts = tags.Posts.OrderByDescending(a => a.PostedOn).ToList();
            ViewBag.TagName = tags.Name;
            return View(posts);
        }
    }
}