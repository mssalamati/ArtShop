using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Blog.Extentions;
using System.Globalization;

namespace Blog.Areas.Admin.Controllers
{

    public class HomeController : BaseController
    {
        [Authorize(Users = "admin")]
        public ActionResult Index()
        {
            string currentCultureName = CultureInfo.CurrentCulture.Name.Substring(0, 2);

            return View();
        }
    }
}