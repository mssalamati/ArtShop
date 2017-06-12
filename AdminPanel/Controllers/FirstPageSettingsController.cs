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
            return View(db.SiteParams.ToList());
        }
    }
}