using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class languagesController : BaseController
    {
        public ActionResult Index()
        {
            var data = db.Languages;
            return View(data.ToList());
        }
    }
}