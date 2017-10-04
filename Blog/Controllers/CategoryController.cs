using Blog.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            string currentCultureName = CultureInfo.CurrentCulture.Name.Substring(0, 2);
            var category = db.Categories.Find(id);
            var posts = category.Posts.Where(a => a.postType == Objects.PostType.Sqr && a.Translations.Any(x => x.languageId == currentCultureName && (x.Description != null))).OrderByDescending(a => a.PostedOn).Take(20).ToList();
            ViewBag.CategoryName = category.Name;
            ViewBag.id = category.Id;
            return View(posts);
        }
        public ActionResult More(int id, int page = 1)
        {
            string currentCultureName = CultureInfo.CurrentCulture.Name.Substring(0, 2);
            var category = db.Categories.Find(id);
            var post = category.Posts.Where(a => a.postType == Objects.PostType.Sqr && a.Translations.Any(x => x.languageId == currentCultureName && (x.Description.Length != 0 && x.Description != null))).OrderByDescending(a => a.PostedOn).Skip((page - 1) * 20).Take(20).ToList();
            return PartialView(post);
        }
    }
}