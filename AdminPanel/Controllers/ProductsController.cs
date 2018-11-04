using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    [Authorize(Roles = "Superadmin,Administrator,Manager")]
    public class ProductsController : BaseController
    {
        public ActionResult Index(int page = 1, string search = "")
        {
            int count = 0, pagesize = 16, take = pagesize, skip = (page - 1) * pagesize;
            var data = db.Products;
                 
            count = data.Count();
            int maxpage = count % pagesize != 0 ? (count / pagesize) + 1 : (count / pagesize);
            ViewBag.page = page; ViewBag.maxpage = maxpage;
            return View(data.OrderByDescending(x => x.CreateDate).Skip(skip).Take(take).ToList());
        }

        public ActionResult detail(int id)
        {
            var obj = db.Products.Find(id);
            return View(obj);
        }

        public ActionResult remove(int id)
        {
            var obj = db.Products.Find(id);
            db.Products.Remove(obj);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}