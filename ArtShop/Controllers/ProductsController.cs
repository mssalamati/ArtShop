using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtShop.Controllers
{
    public class ProductsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Search()
        {
            var p = db.Products.ToList();
            return View(p);
        }

        public ActionResult single(int id)
        {
            var p = db.Products.Find(id);
            return View(p);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}