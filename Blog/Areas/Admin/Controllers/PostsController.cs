using Blog.Areas.Admin.Models.ViewModel;
using Blog.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Areas.Admin.Controllers
{
    public class PostsController : BaseController
    {
        // GET: Admin/Posts
        public ActionResult Index(int page = 1, string search = "")
        {
            int count = 0, pagesize = 15, take = pagesize, skip = (page - 1) * pagesize;
            var data = db.Posts
                 .Where(x => string.IsNullOrEmpty(search) || x.Title.Contains(search))
                 .OrderBy(x => x.PostedOn)
                 .Skip(skip).Take(take);
            count = data.Count();
            int maxpage = count % pagesize != 0 ? (count / pagesize) + 1 : (count / pagesize);
            ViewBag.page = page; ViewBag.maxpage = maxpage; ViewBag.search = search;
            return View(data.ToList());
        }

        public ActionResult Add()
        {
            ViewBag.caregories = db.Categories.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Add(PostViewModel model)
        {          
            return View();
        }

        public ActionResult Edit()
        {
            ViewBag.caregories = db.Categories.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Edit(PostViewModel model)
        {
            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }

    }
}