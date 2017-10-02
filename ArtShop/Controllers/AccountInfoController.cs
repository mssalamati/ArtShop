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
using System.Globalization;

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
            try
            {

                if (userProfile.userLinks != null)
                {
                    model.Facebook = userProfile.userLinks.Facebook;
                    model.Twitter = userProfile.userLinks.Twitter;
                    model.Pinterest = userProfile.userLinks.Pinterest;
                    model.Tumblr = userProfile.userLinks.Tumblr;
                    model.Instagram = userProfile.userLinks.Instagram;
                    model.GooglePlus = userProfile.userLinks.GooglePlus;
                    model.Website = userProfile.userLinks.Website;

                    model.country = userProfile.country;
                    model.City = userProfile.City;
                    model.Region = userProfile.Region;
                    model.ZipCode = userProfile.ZipCode;
                }

                if (userProfile.personalInformation != null)
                {
                    model.AboutMe = string.IsNullOrEmpty(userProfile.personalInformation.Current().AboutMe) ? "" : userProfile.personalInformation.Current().AboutMe;
                    model.Education = string.IsNullOrEmpty(userProfile.personalInformation.Current().Education) ? "" : userProfile.personalInformation.Current().Education;
                    model.Events = string.IsNullOrEmpty(userProfile.personalInformation.Current().Events) ? "" : userProfile.personalInformation.Current().Events;
                    model.Exhibitions = string.IsNullOrEmpty(userProfile.personalInformation.Current().Exhibitions) ? "" : userProfile.personalInformation.Current().Exhibitions;
                }
            }
            catch (Exception ex)
            {
                db.logs.Add(new Log() { date = DateTime.Now, Location = "account info", Message = ex.Message + "   " + ex.InnerException + " " + ex.StackTrace + " ", Type = 1 });
                throw;
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
            string currentCultureName = CultureInfo.CurrentCulture.Name.Substring(0, 2);

            PersonalInformation pi = new PersonalInformation();
            if (userProfile.personalInformation.Translations != null)
            {
                pi.Translations = userProfile.personalInformation.Translations;
            }

            pi.Translations = new List<PersonalInformationTranslation>();
            pi.Translations.Add(new PersonalInformationTranslation { AboutMe = model.AboutMe, Education = model.Education, Events = model.Events, Exhibitions = model.Exhibitions, languageId = currentCultureName });

            userProfile.personalInformation = pi;
            //item.languageId = currentCultureName;
            //item.AboutMe = model.AboutMe;
            //item.Education = model.Education;
            //item.Events = model.Events;
            //item.Exhibitions = model.Exhibitions;



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


        public ActionResult SalesDashboard()
        {
            var userId = User.Identity.GetUserId();
            var userProfile = db.UserProfiles.Find(userId);
            ViewBag.account = userProfile.Account;

            ViewBag.orders = db.OrderDetails.Include("Product").Include("order")
                .Where(x => x.Product.user_id == userId)
                .Where(x => x.order.TransactionDetail.Payed).ToList();

            return View(userProfile.PayoutRequests.ToList());
        }

        [HttpPost]
        public ActionResult SalesDashboard(PayoutRequest model)
        {
            var userId = User.Identity.GetUserId();
            var userProfile = db.UserProfiles.Find(userId);
            ViewBag.account = userProfile.Account;
            if (!userProfile.PayoutRequests.Any(x => x.Seen == false))
            {
                model.Value = userProfile.Account;
                model.date = DateTime.Now;
                userProfile.PayoutRequests.Add(model);
                db.SaveChanges();
            }
            return View(userProfile.PayoutRequests.ToList());
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