using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;


namespace Blog.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {


            return View();
        }

    }
}