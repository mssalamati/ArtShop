using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtShop.Controllers
{
    public class ProductsController : Controller
    {
        public ActionResult Search()
        {
            return View();
        }

        public ActionResult single()
        {
            return View();
        }
    }
}