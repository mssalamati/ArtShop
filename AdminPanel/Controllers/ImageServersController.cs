using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class ImageServersController : BaseController
    {
        public ActionResult Index()
        {
            var parameters = db.SiteObjectParams.FirstOrDefault();

            return View();
        }
    }
}