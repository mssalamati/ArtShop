using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class FirstPageSettingsController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.SiteObjectParams = db.SiteObjectParams.FirstOrDefault();
            ViewBag.SiteParams = db.SiteParams.ToList();
            return View();
        }
    }
}