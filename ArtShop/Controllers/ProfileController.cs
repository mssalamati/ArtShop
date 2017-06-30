using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ArtShop.Models;

namespace ArtShop.Controllers
{
    public class ProfileController : BaseController
    {
        // GET: Profile
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.FirstOrDefault(x => x.ApplicationUserDetail.Id == userId);

            ProfileIndexViewModel model = new ProfileIndexViewModel();

            model.fullName = userProfile.FirstName + " " + userProfile.LastName;

            return View(model);
        }
    }
}