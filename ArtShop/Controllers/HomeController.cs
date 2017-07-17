using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer.Extentions;
using ArtShop.Models;
using ArtShop.Helper;
using System.Globalization;
using System.Configuration;
using DataLayer.Enitities;
using DataLayer;
using Microsoft.AspNet.Identity;
using ArtShop.Util;

namespace ArtShop.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            string currentCultureName = CultureInfo.CurrentCulture.Name.Substring(0, 2);
            var SiteParams = db.SiteParams.Include("Translations").ToList()
                .ToDictionary(x => x.Name,
                y => y.Translations.FirstOrDefault(t => t.languageId == currentCultureName) == null ? string.Empty
                : y.Translations.FirstOrDefault(t => t.languageId == currentCultureName).Value);
            var SiteObjectParams = db.SiteObjectParams.AsQueryable().FirstOrDefault();
            var sliderImage = SiteObjectParams.SliderImage;
            HomeIndexViewModel model = new HomeIndexViewModel();
            model.Slider_Image = ConfigurationManager.AppSettings["FileUrl"] + sliderImage;
            model.slider_H1 = SiteParams[nameof(model.slider_H1).Replace("_", " ")];
            model.slider_H2 = SiteParams[nameof(model.slider_H2).Replace("_", " ")];
            model.slider_Button_Text = SiteParams[nameof(model.slider_Button_Text).Replace("_", " ")];
            model.slider_Button_Url = SiteParams[nameof(model.slider_Button_Url).Replace("_", " ")];
            model.Selected_Art_1 = SiteParams[nameof(model.Selected_Art_1).Replace("_", " ")];
            model.Selected_Art_2 = SiteParams[nameof(model.Selected_Art_2).Replace("_", " ")];
            model.Selected_Art_3 = SiteParams[nameof(model.Selected_Art_3).Replace("_", " ")];
            return View(model);
        }

        
        public ActionResult Header(string culture)
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.FirstOrDefault(x => x.ApplicationUserDetail.Id == userId);
            if (userProfile != null && !string.IsNullOrEmpty(userProfile.FirstName + userProfile.LastName))
            {
                ViewBag.fullName = userProfile.FirstName + " " + userProfile.LastName;
                ViewBag.Title = userProfile.FirstName + " " + userProfile.LastName;
            }
            else
            {
                ViewBag.fullName = User.Identity.GetUserName();
                ViewBag.Title = User.Identity.GetUserName();
            }
            return PartialView("_Header", CashManager.Instance.Header);
        }

        public ActionResult SetCulture(string culture)
        {
            culture = CultureHelper.GetImplementedCulture(culture);
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            string url = this.Request.UrlReferrer.AbsolutePath;
            return Redirect(url);
        }
    }
}