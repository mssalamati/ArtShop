using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ArtShop.Models;
using System.Threading.Tasks;
using DataLayer.Enitities;
using Utilities;

namespace ArtShop.Controllers
{
    public class AccountInfoController : BaseController
    {
        // GET: AccountInfo
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);

            AccountInfoViewModel model = new AccountInfoViewModel();
            model.FirstName = userProfile.FirstName;
            model.LastName = userProfile.LastName;
            model.Email = userProfile.ApplicationUserDetail.Email;
            model.profileType = userProfile.profileType;
            model.ReceiveNewArtEmail = userProfile.ReceiveNewArtEmail;
            model.MailingList = userProfile.MailingList;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(AccountInfoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);

            userProfile.FirstName = model.FirstName;
            userProfile.LastName = model.LastName;
            userProfile.ApplicationUserDetail.Email = model.Email;
            userProfile.profileType = model.profileType;
            userProfile.ReceiveNewArtEmail = model.ReceiveNewArtEmail;
            userProfile.MailingList = model.MailingList;

            db.SaveChanges();

            return View(model);
        }

        public ActionResult ProfileInformation()
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);
            ViewBag.profileType = userProfile.profileType;
            ProfileInformationViewModel model = new ProfileInformationViewModel();
            if (userProfile.userLinks != null)
            {
                model.Facebook = userProfile.userLinks.Facebook;
                model.Twitter = userProfile.userLinks.Twitter;
                model.Pinterest = userProfile.userLinks.Pinterest;
                model.Tumblr = userProfile.userLinks.Tumblr;
                model.Instagram = userProfile.userLinks.Instagram;
                model.GooglePlus = userProfile.userLinks.GooglePlus;
                model.Website = userProfile.userLinks.Website;
                model.AboutMe = userProfile.personalInformation.AboutMe;
                model.Education = userProfile.personalInformation.Education;
                model.Events = userProfile.personalInformation.Events;
                model.Exhibitions = userProfile.personalInformation.Exhibitions;
                model.country = userProfile.country;
                model.City = userProfile.City;
                model.Region = userProfile.Region;
                model.ZipCode = userProfile.ZipCode;
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProfileInformation(ProfileInformationViewModel model)
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);
            userProfile.userLinks = new UserLink();
            userProfile.personalInformation = new PersonalInformation();
            userProfile.userLinks.Facebook = model.Facebook;
            userProfile.userLinks.Twitter = model.Twitter;
            userProfile.userLinks.Pinterest = model.Pinterest;
            userProfile.userLinks.Tumblr = model.Tumblr;
            userProfile.userLinks.Instagram = model.Instagram;
            userProfile.userLinks.GooglePlus = model.GooglePlus;
            userProfile.userLinks.Website = model.Website;
            userProfile.personalInformation.AboutMe = model.AboutMe;
            userProfile.personalInformation.Education = model.Education;
            userProfile.personalInformation.Events = model.Events;
            userProfile.personalInformation.Exhibitions = model.Exhibitions;
            userProfile.countryId = model.countryId;
            userProfile.City = model.City;
            userProfile.Region = model.Region;
            userProfile.ZipCode = model.ZipCode;

            db.SaveChanges();

            return View(model);
        }

        public ActionResult UploadAvatar()
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);
            ViewBag.profileType = userProfile.profileType;

            return View();
        }

        [HttpPost]
        public ActionResult UploadAvatar(HttpPostedFileBase Image)
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);
            ViewBag.profileType = userProfile.profileType;

            string tempFolderName = "Upload/profile_Images";
            var result = ImageHelper.Saveimage(Server, Image, tempFolderName, ImageHelper.saveImageMode.Not);
            if (result.ResultStatus)
            {
                userProfile.PhotoPath = result.FullPath;
                db.SaveChanges();
            }
            return RedirectToAction("Index","profile");
        }

        public ActionResult Billing()
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);
            ViewBag.profileType = userProfile.profileType;
            if (userProfile.billingInfo != null)
            {
                return View(userProfile.billingInfo);
            }


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Billing(BillingInfo model)
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);

            userProfile.billingInfo = new BillingInfo();
            userProfile.billingInfo.CountryId= model.CountryId;
            userProfile.billingInfo.Street = model.Street;
            userProfile.billingInfo.Unit = model.Unit;
            userProfile.billingInfo.City = model.City;
            userProfile.billingInfo.Region = model.Region;
            userProfile.billingInfo.ZipCode = model.ZipCode;
            userProfile.billingInfo.PhoneNumber = model.PhoneNumber;
            
            db.SaveChanges();

            return View(model);
        }

        public ActionResult Orders()
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);
            ViewBag.profileType = userProfile.profileType;
       
            return View();
        }
    }

}