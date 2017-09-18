using Blog.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class CategoryController : BaseController
    {
        // GET: Category
        public ActionResult Index(int id)
        {
            var category = db.Categories.Find(id);
            var posts = category.Posts.OrderByDescending(a => a.PostedOn).Take(20).ToList();
            ViewBag.CategoryName = category.Name;
            ViewBag.id = category.Id;
            return View(posts);
        }
        public ActionResult More(int id,int page = 1)
        {
            var category = db.Categories.Find(id);
            var post = category.Posts.OrderByDescending(a => a.PostedOn).Skip((page - 1) * 20).Take(20).ToList();
            return PartialView(post);
        }
    }
}