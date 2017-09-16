﻿using Blog.Areas.Admin.Controllers;
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
            var posts = category.Posts.OrderByDescending(a => a.PostedOn).ToList();
            ViewBag.CategoryName = category.Name;
            return View(posts);
        }
    }
}