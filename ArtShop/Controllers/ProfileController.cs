using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ArtShop.Models;
using System.Threading.Tasks;
using DataLayer.Enitities;

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

        public ActionResult Collection()
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.FirstOrDefault(x => x.ApplicationUserDetail.Id == userId);

            ProfileIndexViewModel model = new ProfileIndexViewModel();

            model.fullName = userProfile.FirstName + " " + userProfile.LastName;

            return View(model);
        }

        public ActionResult NewCollection()
        {
            NewCollectionViewModel model = new NewCollectionViewModel();
     
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewCollection(NewCollectionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.CollectionTitle))
            {
                Collection collection = new Collection();
                collection.Title = model.CollectionTitle;
                collection.Description = model.CollectionDescription;
                collection.IsPrivate = model.IsPrivate;
                collection.Type = model.CollectionType;

                var userId = User.Identity.GetUserId();

                var userProfile = db.UserProfiles.FirstOrDefault(x => x.ApplicationUserDetail.Id == userId);

                userProfile.Collections.Add(collection);

                db.SaveChanges();
            }
            else
            {
                ModelState.AddModelError("", "Please choose a title");
                return View(model);
            }
             
            return RedirectToAction("Collection");
        }
    }
}