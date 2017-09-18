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
using DataLayer.Extentions;
using ArtShop.Util;
using RestSharp;

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
            if (model.MailingList && !userProfile.MailingList)
            {
                AddSubscriber(userProfile.ApplicationUserDetail.Email);
            }
            else if (!model.MailingList)
            {
                RemoveFromSubscribers(userProfile.ApplicationUserDetail.Email);
            }
            userProfile.MailingList = model.MailingList;

            db.SaveChanges();

            return View(model);
        }

        public void AddSubscriber(string email)
        {
            var client = new RestClient("https://api.mailerlite.com/api/v2/groups/7737389/subscribers");
            var request = new RestRequest(Method.POST);
            request.AddHeader("x-mailerlite-apikey", "0e0ba56cc888feb4f4573cfe0a5f497c");
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"email\":\"" + email + "\", \"name\": \" \", \"fields\": {\"company\": \"Artiscovery\"}}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
        }

        public void RemoveFromSubscribers(string email)
        {

            var client = new RestClient("https://api.mailerlite.com/api/v2/groups/7737389/subscribers/" + email);
            var request = new RestRequest(Method.POST);
            request.AddHeader("x-mailerlite-apikey", "0e0ba56cc888feb4f4573cfe0a5f497c");
            //request.AddHeader("content-type", "application/json");
            //request.AddParameter("application/json", "{\"" + email + "\"}", ParameterType.QueryString);
            //request.AddQueryParameter("", email);

            IRestResponse response = client.Delete(request);
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
        public ActionResult ProfileInformation(ProfileInformationViewModel model)
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);
            userProfile.userLinks = new UserLink();
            userProfile.personalInformation = new PersonalInformation();
            userProfile.userLinks.Facebook = model.Facebook != null ? (model.Facebook.Contains("http") ? model.Facebook : "https://" + model.Facebook) : model.Facebook;
            userProfile.userLinks.Twitter = model.Twitter != null ? (model.Twitter.Contains("http") ? model.Twitter : "https://" + model.Twitter) : model.Twitter;
            userProfile.userLinks.Pinterest = model.Pinterest != null ? (model.Pinterest.Contains("http") ? model.Pinterest : "https://" + model.Pinterest) : model.Pinterest;
            userProfile.userLinks.Tumblr = model.Tumblr != null ? (model.Tumblr.Contains("http") ? model.Tumblr : "https://" + model.Tumblr) : model.Tumblr;
            userProfile.userLinks.Instagram = model.Instagram != null ? (model.Instagram.Contains("http") ? model.Instagram : "https://" + model.Instagram) : model.Instagram;
            userProfile.userLinks.GooglePlus = model.GooglePlus != null ? (model.GooglePlus.Contains("http") ? model.GooglePlus : "https://" + model.GooglePlus) : model.GooglePlus;
            userProfile.userLinks.Website = model.Website != null ? (model.Website.Contains("http") ? model.Website : "http://" + model.Website) : model.Website;
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
            try
            {
                string tempFolderName = "Upload/profile_Images";
                var result = ImageHelper.Saveimage(Server, Image, tempFolderName, ImageHelper.saveImageMode.Not);
                if (result.ResultStatus)
                {
                    userProfile.PhotoPath = result.FullPath;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                db.logs.Add(new Log() { date = DateTime.Now, Location = "upload", Message = ex.Message + "   " + ex.InnerException + " " + ex.StackTrace + " ", Type = 0 });
                db.SaveChanges();
                throw;
            }
            return RedirectToAction("Index", "profile");
        }

        public ActionResult Billing()
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);
            ViewBag.profileType = userProfile.profileType;
        
            if (userProfile.billingInfo != null)
            {
                ViewBag.country = userProfile.billingInfo.country != null ? userProfile.billingInfo.country.Current().Name : "iran";
                return View(userProfile.billingInfo);
            }


            return View(new BillingInfo());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Billing(BillingInfo model)
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);

            userProfile.billingInfo = new BillingInfo();
            userProfile.billingInfo.CountryId = model.CountryId;
            userProfile.billingInfo.Street = model.Street;
            userProfile.billingInfo.Unit = model.Unit;
            userProfile.billingInfo.City = model.City;
            userProfile.billingInfo.Region = model.Region;
            userProfile.billingInfo.ZipCode = model.ZipCode;
            userProfile.billingInfo.PhoneNumber = model.PhoneNumber;

            db.SaveChanges();
            ViewBag.country = CashManager.Instance.Countries.FirstOrDefault(a => a.Key == model.CountryId).Value;

            return View(model);
        }

        public ActionResult Orders(int page = 1)
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);
            ViewBag.profileType = userProfile.profileType;
            var data = db.Orders.Include("TransactionDetail").Where(x => x.user_id == userId).OrderByDescending(o => o.BuyDate).Skip(10 * (page - 1)).Take(10);

            return View(data);
        }


        public ActionResult UploadID()
        {


            return View();
        }

        [HttpPost]
        public ActionResult UploadID(HttpPostedFileBase Image)
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);

            string tempFolderName = "Upload/goverment-ids";
            var result = ImageHelper.Saveimage(Server, Image, tempFolderName, ImageHelper.saveImageMode.Not);
            if (result.ResultStatus)
            {
                userProfile.GovermentIdPath = result.FullPath;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "profile");
        }
    }

}