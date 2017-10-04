using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtShop.Controllers
{
    public class SupportController : BaseController
    {
        // GET: Support
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Header()
        {
            
            return PartialView("_Header");
        }
    }
}