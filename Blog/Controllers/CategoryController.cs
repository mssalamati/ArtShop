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

            var posts = category.Posts.Where(a => a.postType == Objects.PostType.Sqr && a.Translations.Any(x => x.languageId == currentCultureName && (x.Description != null))).OrderByDescending(a => a.PostedOn).Take(12).ToList();
            ViewBag.CategoryName = category.Name;
            ViewBag.id = category.Id;
            ViewBag.Category = category;
            ViewBag.isSub = false;
            return View(posts);


        }
        public ActionResult SubCategory(int id)
        {
            string currentCultureName = CultureInfo.CurrentCulture.Name.Substring(0, 2);
            var subCategory = db.SubCategories.Find(id);
            var posts = subCategory.Posts.Where(a => a.postType == Objects.PostType.Sqr && a.Translations.Any(x => x.languageId == currentCultureName && (x.Description != null))).OrderByDescending(a => a.PostedOn).Take(12).ToList();
            ViewBag.CategoryName = subCategory.Name;
            ViewBag.id = subCategory.Id;
            ViewBag.isSub = true;
            ViewBag.SubCategory = subCategory;
            return View(posts);

        }
        public ActionResult More(int id, int page = 1, bool isSub = false)
        {
            string currentCultureName = CultureInfo.CurrentCulture.Name.Substring(0, 2);
            var category = db.Categories.Find(id);
            if (!isSub)
            {
                var post = category.Posts.Where(a => a.postType == Objects.PostType.Sqr && a.Translations.Any(x => x.languageId == currentCultureName && (x.Description != null))).OrderByDescending(a => a.PostedOn).Skip((page - 1) * 12).Take(12).ToList();

                return PartialView(post);
            }
            else
            {
                var subCategory = db.SubCategories.Find(id);
                var post = subCategory.Posts.Where(a => a.postType == Objects.PostType.Sqr && a.Translations.Any(x => x.languageId == currentCultureName && (x.Description != null))).OrderByDescending(a => a.PostedOn).Skip((page - 1) * 12).Take(12).ToList();

                return PartialView(post);
            }
        }
    }
}