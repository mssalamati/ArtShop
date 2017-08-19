﻿using System;
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
            HomeIndexViewModel model = new HomeIndexViewModel();
            var count = db.sliderImages.Count();
            if (count != 0)
            {
                var randomNumber = new Random().Next(0, count);
                var sl = db.sliderImages.Include("Translations").ToList().Skip(randomNumber).Take(1).FirstOrDefault();
                model.Slider_Image = ConfigurationManager.AppSettings["FileUrl"] + "/" + sl.path;
                model.slider_H1 = sl.Current().H1;
                model.slider_H2 = sl.Current().H2;
                model.slider_Button_Text = sl.Current().ButtonText;
                model.slider_Button_Url = sl.ButtonURL;
                model.slider_text_color = sl.TextColor;
                model.slider_Button_color = sl.ButtonColor;
                model.slider_Button_text_color = sl.ButtonTextColor;
                model.slider_P = sl.Current().P1;
                model.FirstPageSections = db.FirstPageSections.Include("Translations").ToList();
            }

            return View(model);
        }

        public ActionResult _SelectedCurators(FirstPageSection model)
        {
            try
            {
                ViewBag.pic1 = db.Products.Find(int.Parse(model.param1)).Sqphoto.Path;
                ViewBag.pic2 = db.Products.Find(int.Parse(model.param2)).Sqphoto.Path;
                ViewBag.pic3 = db.Products.Find(int.Parse(model.param3)).Sqphoto.Path;
            }
            catch
            {

            }
            return PartialView(model);
        }
        public ActionResult _SalebyPrice(FirstPageSection model)
        {
            return PartialView(model);
        }
        public ActionResult _RecentlySold(FirstPageSection model)
        {
            return PartialView(model);
        }
        public ActionResult _SalebyStyle(FirstPageSection model)
        {
            return PartialView(model);
        }
        public ActionResult _SalebyCategory(FirstPageSection model)
        {
            return PartialView(model);
        }

        public ActionResult Header()
        {
            var userId = User.Identity.GetUserId();
            var cart = CartManager.GetCart(this.HttpContext);
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
            ViewBag.card = cart.GetCartItems().Count;
            return PartialView("_Header", CashManager.Instance.Header);
        }

        public ActionResult Footer()
        {
            return PartialView("_footer", CashManager.Instance.Footer);
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

        public ActionResult cashmanager(string id)
        {
            if (id == "soroosh1313")
                CashManager.resete();
            return Content("ok");
        }
    }
}