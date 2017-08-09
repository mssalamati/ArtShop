using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtShop.Controllers
{
    public class PagesController : Controller
    {
        public ActionResult About()
        {
            return View();
        }
    }
}