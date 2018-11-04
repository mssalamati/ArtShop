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
        [Authorize(Roles = "Superadmin,Administrator,Manager")]
        public ActionResult Index()
        {
            ViewBag.product = db.Products.Count();
            ViewBag.Materials = db.Materials.Count();
            ViewBag.Subjects = db.Subjects.Count();
            ViewBag.Mediums = db.Mediums.Count();
            ViewBag.Styles = db.Styles.Count();
            ViewBag.Users = db.Users.Count();
            ViewBag.artist = db.UserProfiles.Where(a => a.profileType == DataLayer.Enitities.ProfileType.Artist).Count();
            ViewBag.coolector = db.UserProfiles.Where(a => a.profileType == DataLayer.Enitities.ProfileType.Collector).Count();

            ViewBag.RecentIDs = db.UserProfiles.Where(x => !string.IsNullOrEmpty(x.GovermentIdPath))
                  .OrderByDescending(x => x.isIDConfirmed).Take(6).ToList();
            ViewBag.RecentArtworks = db.Products.OrderByDescending(x => x.CreateDate).Take(8).ToList();

            return View();
        }

    }
}