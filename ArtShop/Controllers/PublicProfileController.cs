using ArtShop.Models;
using DataLayer.Enitities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtShop.Controllers
{
    public class PublicProfileController : BaseController
    {
        // GET: PublicProfile
        public ActionResult Index(string id)
        {
            var userProfile = db.UserProfiles.FirstOrDefault(x => x.ApplicationUserDetail.Id == id);

            ProfileIndexViewModel model = new ProfileIndexViewModel();
            model.id = userProfile.Id;
            model.fullName = userProfile.FirstName + " " + userProfile.LastName;
            model.artworkCount = userProfile.Products.Count;
            model.collectionsCount = userProfile.Collections.Count;
            model.favoritesCount = userProfile.Favorits.Count;
            model.city = userProfile.City == null ? " " : userProfile.City;
            model.region = userProfile.Region == null ? " " : userProfile.Region;
            model.country = userProfile.country;
            model.photoPath = userProfile.PhotoPath;
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


            model.artworks = new List<Product>();
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

            return View(model);
        }

        public ActionResult Collection(string id)
        {
            var userProfile = db.UserProfiles.Find(id);
            ViewBag.profileFullName = userProfile.FirstName + " " + userProfile.LastName;
            ViewBag.artworksCount = userProfile.Products.Count;
            ViewBag.favoritesCount = userProfile.Favorits.Count;
            ViewBag.id = id;
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

        public ActionResult CollectionView(string userId, int id)
        {

            var userProfile = db.UserProfiles.Find(userId);
            ViewBag.ProfileFullName = userProfile.FirstName + " " + userProfile.LastName;
            ViewBag.id = userId;
            var collection = userProfile.Collections.FirstOrDefault(x => x.Id == id);
            ViewBag.CollectionName = collection.Title;
            ViewBag.CollectionId = collection.Id;

            if (collection.Artworks != null)
                return View(collection.Artworks);

            return View();
        }

        public ActionResult Favorites(string id)
        {
            var userProfile = db.UserProfiles.Find(id);
            ViewBag.ProfileFullName = userProfile.FirstName + " " + userProfile.LastName;
            ViewBag.collectionCount = userProfile.Collections.Count;
            ViewBag.artworkCount = userProfile.Products.Count;
            ViewBag.id = id;
            if (userProfile.Favorits != null)
                return View(userProfile.Favorits);

            return View();
        }

        public ActionResult ArtWorks(string id)
        {
            var userProfile = db.UserProfiles.Find(id);
            ViewBag.ProfileFullName = userProfile.FirstName + " " + userProfile.LastName;
            ViewBag.favoritesCount = userProfile.Favorits.Count;
            ViewBag.collectionCount = userProfile.Collections.Count;
            ViewBag.id = id;

            return View(userProfile.Products);
        }
    }
}