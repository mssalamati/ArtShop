using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class ProductsController : BaseController
    {
        public ActionResult Index(int page = 1, string search = "")
        {
            var data = db.Products;
            return View(data.ToList());
        }
    }
}