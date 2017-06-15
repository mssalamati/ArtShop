using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using AdminPanel.Models.ViewModel;

namespace AdminPanel.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.product = db.Products.Count();
            ViewBag.Materials = db.Materials.Count();
            ViewBag.Subjects = db.Subjects.Count();
            ViewBag.Mediums = db.Mediums.Count();
            ViewBag.Styles = db.Styles.Count();
            ViewBag.Users = db.Users.Count();
            ViewBag.artist = db.UserProfiles.Count();
            ViewBag.coolector = db.UserProfiles.Count();

            return View();
        }

    }
}