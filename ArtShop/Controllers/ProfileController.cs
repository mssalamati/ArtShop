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
            model.artworkCount = userProfile.Products.Count;
            model.collectionsCount = userProfile.Collections.Count;
            model.city = userProfile.City;
            model.region = userProfile.Region;

            if (userProfile.country != null)
            {
                model.country = userProfile.country;
            }
            else
            {
                model.country = new Country { Code = " " };
            }

            model.artworks = new List<Product>();
            int counter = 0;

            foreach (var item in userProfile.Products)
            {
                model.artworks.Add(item);
                counter++;
            }

            if (counter < 3 && counter != 0)
            {
                for (int i = 0; i < 4 - counter; i++)
                {
                    Product p = new Product();
                    model.artworks.Add(p);
                }
            }

            return View(model);
        }

        public ActionResult Collection()
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);
            ViewBag.profileFullName = userProfile.FirstName + " " + userProfile.LastName;
            ViewBag.artworksCount = userProfile.Products.Count;

            List<CollectionViewModel> collectionViewModel = new List<CollectionViewModel>();

            int counter = 0;

            foreach (var item in userProfile.Collections)
            {
                CollectionViewModel model = new CollectionViewModel();
                model.CollectionId = item.Id;
                model.CollectionName = item.Title;
                model.collectionProduct = new List<CollectionProduct>();
                foreach (var art in item.Artworks)
                {
                    model.collectionProduct.Add(art);
                    counter++;
                }

                if (counter < 3 )
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

            CollectionViewModel model = new CollectionViewModel();
            model.CollectionId = collection.Id;
            model.CollectionName = collection.Title;
            model.collectionProduct = collection.Artworks.ToList();


            return View(model);
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
            if (userProfile.Favorits != null)
                return View(userProfile.Favorits);

            return View();
        }

        public ActionResult ArtWorks()
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);
            ViewBag.ProfileFullName = userProfile.FirstName + " " + userProfile.LastName;

            ViewBag.collectionCount = userProfile.Collections.Count;


            return View(userProfile.Products);
        }
    }
}