using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Areas.Admin.Controllers
{
    public class FirstPageController : BaseController
    {
        public ActionResult Index()
        {
            var model = db.NavigationCategories.Include("category").OrderBy(x => x.priority);
            ViewBag.categories = db.Categories.ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(int id)
        {
            if (db.NavigationCategories.Any(x => x.categoryId == id))
            {
                ViewBag.error = "این دسته بندی تکراری است";
                return Redirect("/admin/FirstPage/Index");
            }
            var priority = db.NavigationCategories.Count() == 0 ? 0 : db.NavigationCategories.Max(x => x.priority) + 1;
            var nav = new NavigationCategory()
            {
                categoryId = id,
                priority = priority
            };
            db.NavigationCategories.Add(nav);
            ViewBag.success = "ثبت شد";
            db.SaveChanges();
            return Redirect("/admin/FirstPage/Index");
        }

        public ActionResult MoveUp(int id)
        {
            var finder = db.NavigationCategories.Find(id);
            var next = db.NavigationCategories.SingleOrDefault(x => x.priority == finder.priority + 1);
            if (next != null)
            {
                next.priority -= 1;
                finder.priority += 1;
            }
            db.SaveChanges();
            return Redirect("/admin/FirstPage/Index");
        }

        public ActionResult MoveDown(int id)
        {
            var finder = db.NavigationCategories.Find(id);
            var pre = db.NavigationCategories.SingleOrDefault(x => x.priority == finder.priority - 1);
            if (pre != null)
            {
                pre.priority += 1;
                finder.priority -= 1;
            }
            db.SaveChanges();
            return Redirect("/admin/FirstPage/Index");
        }

        public ActionResult Delete(int id)
        {
            var finder = db.NavigationCategories.Find(id);
            db.NavigationCategories.Remove(finder);
            db.SaveChanges();
            return Redirect("/admin/FirstPage/Index");
        }

    }
}