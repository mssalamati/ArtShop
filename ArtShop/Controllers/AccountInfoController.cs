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
    [Authorize]
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
            ViewBag.ischanged = false;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(AccountInfoViewModel model)
        {
            ViewBag.ischanged = true;
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
            ViewBag.ischanged = false;
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

                    model.countryId = userProfile.country == null ? 0 : userProfile.country.Id;
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

            if (userProfile.personalInformation == null)
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
            ViewBag.ischanged = true;

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
            return RedirectToActionPermanent("Index", "profile");
        }

        public ActionResult DeleteAvatar()
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);
            userProfile.PhotoPath = "";
            db.SaveChanges();

            return View();
        }

        public ActionResult Billing()
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);
            ViewBag.profileType = userProfile.profileType;
            ViewBag.ischanged = false;
            ViewBag.ConfirmationStatus = userProfile.IDStatus;
            if (userProfile.billingInfo != null)
            {
                ViewBag.country = userProfile.billingInfo.country != null ? userProfile.billingInfo.country.Current().Name : "Iran";
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
            ViewBag.ischanged = true;
            ViewBag.ConfirmationStatus = userProfile.IDStatus;
            return View(model);
        }

        public ActionResult Orders(int page = 1)
        {
            int pageSize = 10;
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);
            ViewBag.profileType = userProfile.profileType;
            var data = db.Orders.Include("TransactionDetail").Where(x => x.user_id == userId).OrderByDescending(o => o.BuyDate).AsQueryable();


            var count = data.Count();
            page = Math.Min(page, (int)Math.Ceiling((float)count / (float)pageSize));
            page = Math.Max(1, page);
            ViewBag.page = page;
            ViewBag.count = count;
            ViewBag.pageSize = pageSize;

            data = data.Skip((page - 1) * pageSize).Take(pageSize);

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
            ViewBag.Seen = true;
            return View(userProfile.PayoutRequests.OrderByDescending(a => a.date).ToList());
        }

        [HttpPost]
        public ActionResult SalesDashboard(PayoutRequest model)
        {
            var userId = User.Identity.GetUserId();
            var userProfile = db.UserProfiles.Find(userId);
            ViewBag.account = userProfile.Account;
            if (!userProfile.PayoutRequests.Any(x => x.Seen == false))
            {
                ViewBag.Seen = true;
                model.Value = userProfile.Account;
                model.date = DateTime.Now;
                userProfile.PayoutRequests.Add(model);
                db.SaveChanges();
            }
            else
                ViewBag.Seen = false;
            return View(userProfile.PayoutRequests.ToList());
        }

        public ActionResult UploadID()
        {


            return View();
        }

        public class ID
        {
            public string LegalName { get; set; }
            public HttpPostedFileBase Image { get; set; }
        }

        [HttpPost]
        public ActionResult UploadID(ID model)
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);
            userProfile.IDStatus = IDCardStatus.Pending;
            string tempFolderName = "Upload/goverment-ids";
            var result = ImageHelper.Saveimage(Server, model.Image, tempFolderName, ImageHelper.saveImageMode.Not);
            if (result.ResultStatus)
            {
                userProfile.GovermentIdPath = result.FullPath;
                userProfile.LegalName = model.LegalName;
                db.SaveChanges();
            }
            return RedirectToActionPermanent("Index", "profile");
        }
    }

}