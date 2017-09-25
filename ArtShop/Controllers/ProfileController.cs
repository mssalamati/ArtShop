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
    [Authorize]
    public class ProfileController : BaseController
    {
        // GET: Profile
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.FirstOrDefault(x => x.ApplicationUserDetail.Id == userId);
            ProfileIndexViewModel model = new ProfileIndexViewModel();
            try
            {
                model.fullName = userProfile.FirstName + " " + userProfile.LastName;
                model.artworkCount = userProfile.Products.Count;
                model.collectionsCount = userProfile.Collections.Count;
                model.favoritesCount = userProfile.Favorits.Count;
                model.city = userProfile.City == null ? " " : userProfile.City;
                model.region = userProfile.Region == null ? " " : userProfile.Region;
                model.country = userProfile.country == null ? new Country() : userProfile.country;
                model.photoPath = userProfile.PhotoPath == null ? "" : userProfile.PhotoPath;
                model.id = userId;

                if (userProfile.userLinks != null)
                {
                    model.facebook = userProfile.userLinks.Facebook == null ? "" : userProfile.userLinks.Facebook;
                    model.twitter = userProfile.userLinks.Twitter == null ? "" : userProfile.userLinks.Twitter;
                    model.pinterest = userProfile.userLinks.Pinterest == null ? "" : userProfile.userLinks.Pinterest;
                    model.tumbler = userProfile.userLinks.Tumblr == null ? "" : userProfile.userLinks.Tumblr;
                    model.instagram = userProfile.userLinks.Instagram == null ? "" : userProfile.userLinks.Instagram;
                    model.googlePlus = userProfile.userLinks.GooglePlus == null ? "" : userProfile.userLinks.GooglePlus;
                    model.myWebsite = userProfile.userLinks.Website == null ? "" : userProfile.userLinks.Website;
                }

                if (userProfile.personalInformation != null)
                {
                    model.aboutme = userProfile.personalInformation.AboutMe == null ? "" : userProfile.personalInformation.AboutMe;
                    model.events = userProfile.personalInformation.Events == null ? "" : userProfile.personalInformation.Events;
                    model.education = userProfile.personalInformation.Education == null ? "" : userProfile.personalInformation.Education;
                    model.Exhibitions = userProfile.personalInformation.Exhibitions == null ? "" : userProfile.personalInformation.Exhibitions;
                }

                try
                {
                    model.artworks = new List<Product>();
                }
                catch (Exception ex)
                {
                    db.logs.Add(new Log() { date = DateTime.Now, Location = "profile", Message = ex.Message + "   " + ex.InnerException + " " + ex.StackTrace + " ", Type = 1 });
                    throw;
                }


                int counter = 0;

                foreach (var item in userProfile.Products)
                {
                    if (counter < 4)
                    {
                        model.artworks.Add(item);
                        counter++;
                    }
                    else
                        break;

                }

                if (counter < 4 && counter != 0)
                {
                    for (int i = 0; i < 4 - counter; i++)
                    {
                        Product p = new Product();
                        model.artworks.Add(p);
                    }
                }
            }
            catch (Exception ex)
            {
                db.logs.Add(new Log() { date = DateTime.Now, Location = "profile", Message = ex.Message + "   " + ex.InnerException + " " + ex.StackTrace + " ", Type = 0 });
                db.SaveChanges();
                throw;
            }


            return View(model);
        }

        public ActionResult Collection()
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);
            ViewBag.profileFullName = userProfile.FirstName + " " + userProfile.LastName;
            ViewBag.artworksCount = userProfile.Products.Count;
            ViewBag.favoritesCount = userProfile.Favorits.Count;
            ViewBag.PhotoPath = userProfile.PhotoPath;
            List<CollectionViewModel> collectionViewModel = new List<CollectionViewModel>();

            int counter = 0;

            foreach (var item in userProfile.Collections)
            {
                CollectionViewModel model = new CollectionViewModel();
                model.CollectionId = item.Id;
                model.CollectionName = item.Title;
                model.CollectionProductCount = item.Artworks.Count;
                model.collectionProduct = new List<CollectionProduct>();
                foreach (var art in item.Artworks)
                {
                    model.collectionProduct.Add(art);
                    counter++;
                }

                if (counter < 3)
                {
                    for (int i = 0; i < 4 - counter; i++)
                    {
                        CollectionProduct p = new CollectionProduct();
                        model.collectionProduct.Add(p);
                    }

                    counter = 0;
                }
                collectionViewModel.Add(model);
            }

            return View(collectionViewModel);
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

        public ActionResult CollectionView(int id)
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);
            ViewBag.ProfileFullName = userProfile.FirstName + " " + userProfile.LastName;

            var collection = userProfile.Collections.FirstOrDefault(x => x.Id == id);
            ViewBag.CollectionName = collection.Title;
            ViewBag.CollectionId = collection.Id;

            if (collection.Artworks != null)
                return View(collection.Artworks);

            return View();
        }

        public ActionResult DeleteCollection(int id)
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);
            ViewBag.ProfileFullName = userProfile.FirstName + " " + userProfile.LastName;

            var collection = userProfile.Collections.FirstOrDefault(x => x.Id == id);

            userProfile.Collections.Remove(collection);
            db.SaveChanges();

            return RedirectToAction("Collection");
        }

        public ActionResult EditCollection(int id)
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);
            ViewBag.ProfileFullName = userProfile.FirstName + " " + userProfile.LastName;

            var collection = userProfile.Collections.FirstOrDefault(x => x.Id == id);

            NewCollectionViewModel model = new NewCollectionViewModel();
            model.CollectionId = collection.Id;
            model.CollectionTitle = collection.Title;
            model.collectionProduct = collection.Artworks;
            model.CollectionDescription = collection.Description;
            model.IsPrivate = collection.IsPrivate;
            model.CollectionType = collection.Type;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCollection(NewCollectionViewModel model)
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);
            ViewBag.ProfileFullName = userProfile.FirstName + " " + userProfile.LastName;

            var collection = userProfile.Collections.FirstOrDefault(x => x.Id == model.CollectionId);

            collection.Title = model.CollectionTitle;
            collection.Description = model.CollectionDescription;
            collection.IsPrivate = model.IsPrivate;
            collection.Type = model.CollectionType;

            db.SaveChanges();

            return RedirectToAction("Collection");
        }

        public ActionResult Favorites()
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);
            ViewBag.ProfileFullName = userProfile.FirstName + " " + userProfile.LastName;
            ViewBag.collectionCount = userProfile.Collections.Count;
            ViewBag.artworkCount = userProfile.Products.Count;
            ViewBag.PhotoPath = userProfile.PhotoPath;
            if (userProfile.Favorits != null)
                return View(userProfile.Favorits);

            return View();
        }

        public ActionResult ArtWorks()
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);
            ViewBag.ProfileFullName = userProfile.FirstName + " " + userProfile.LastName;
            ViewBag.favoritesCount = userProfile.Favorits.Count;
            ViewBag.collectionCount = userProfile.Collections.Count;
            ViewBag.PhotoPath = userProfile.PhotoPath;

            return View(userProfile.Products.OrderByDescending(a => a.CreateDate).ToList());
        }

        public ActionResult ManageArtWorks()
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);
            ViewBag.ProfileFullName = userProfile.FirstName + " " + userProfile.LastName;


            return View();
        }

    }
}