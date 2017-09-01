using Blog.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class PostController : BaseController
    {
        // GET: Post
        public ActionResult Index(int id)
        {
            var post = db.Posts.Find(id);
            return View(post);
        }
    }
}