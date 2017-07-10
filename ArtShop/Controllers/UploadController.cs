﻿using ArtShop.Models;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;
using DataLayer.Extentions;
using System.Configuration;
using ArtShop.Util;
using Microsoft.AspNet.Identity;
using DataLayer.Enitities;

namespace ArtShop.Controllers
{
    [Authorize]
    public class UploadController : BaseController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult upload(HttpPostedFileBase file)
        {
            string tempFolderName = "Upload/products/temps";
            var result = ImageHelper.Saveimage(Server, file, tempFolderName, ImageHelper.saveImageMode.Not);
            if (!result.ResultStatus)
                return Json(new { result = false, data = result.Error });

            return Json(new { result = true, data = result.FullPath });
        }

        //upload picture
        public ActionResult Setep1()
        {
            ViewBag.lastpic = (string)Session["imageAddress"];
            return PartialView();
        }
        [HttpPost]
        public ActionResult Setep1(UploadViewModel.step1 model)
        {
            Session["imageAddress"] = model.img;
            if (string.IsNullOrEmpty(model.img) || !System.IO.File.Exists(HttpContext.Server.MapPath(model.img)))
            {
                ViewBag.Error = Resources.UploadRes.Image_cannot_be_empty;
                return PartialView();
            }
            return RedirectToAction("Setep2");
        }

        // category and subject
        public ActionResult Setep2()
        {
            ViewBag.subjects = CashManager.Instance.Subjects;
            ViewBag.category = CashManager.Instance.Categories;
            return PartialView();
        }
        [HttpPost]
        public ActionResult Setep2(UploadViewModel.step2 model)
        {
            if (model.category == 0 || model.subject == 0)
            {
                ViewBag.Error = Resources.UploadRes.select_subject;
                return PartialView();
            }
            Session["category"] = model.category;
            Session["subject"] = model.subject;
            return RedirectToAction("Setep3");
        }

        //year,forsale,print and copyright
        public ActionResult Setep3()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Setep3(UploadViewModel.step3 model)
        {
            if (model.copyright == false)
            {
                ViewBag.Error = Resources.UploadRes.copyright_error;
                return PartialView();
            }
            if (Session["imageAddress"] == null)
                return RedirectToAction("Setep1");

            Session["copyright"] = model.copyright;
            Session["createYear"] = model.createYear;
            Session["isOrginal"] = model.isOrginal;
            Session["printAvable"] = model.printAvable;
            return RedirectToAction("Setep4");
        }

        //RESIZE PICTURE
        public ActionResult Setep4()
        {

            ViewBag.img = (string)Session["imageAddress"];
            return PartialView();
        }
        [HttpPost]
        public ActionResult Setep4(UploadViewModel.step4 model)
        {
            string image = (string)Session["imageAddress"];
            var result = ImageHelper.Crop(Server, image, model.square_x, model.square_y, model.square_width, model.square_height, model.wide_x, model.wide_y, model.wide_width, model.wide_height);

            if (result.ResultStatus)
            {
                Session["WideFullPath"] = result.WideFullPath;
                Session["SqureFullPath"] = result.SqureFullPath;
            }
            else
            {
                ViewBag.img = (string)Session["imageAddress"];
                return PartialView();
            }

            return RedirectToAction("Setep5");
        }

        //medum,material,style and keywords
        public ActionResult Setep5()
        {
            ViewBag.lastpic = Session["WideFullPath"];
            ViewBag.Mediums = CashManager.Instance.Mediums;
            ViewBag.Materials = CashManager.Instance.Materials;
            ViewBag.Styles = CashManager.Instance.Styles;

            return PartialView();
        }
        [HttpPost]
        public ActionResult Setep5(UploadViewModel.step5 model)
        {
            if (string.IsNullOrEmpty(model.Keywords) || model.Materials.Length == 0 || string.IsNullOrEmpty(model.Mediums) || string.IsNullOrEmpty(model.Styles))
            {
                ViewBag.lastpic = Session["imageAddress"];
                ViewBag.Mediums = CashManager.Instance.Mediums;
                ViewBag.Materials = CashManager.Instance.Materials;
                ViewBag.Styles = CashManager.Instance.Styles;
                ViewBag.error = Resources.UploadRes.Empty_Error;
                return PartialView();
            }

            if (model.Keywords.Split(',').Count() < 5)
            {
                ViewBag.error = Resources.UploadRes.keyword_lenght_error;
                return PartialView();
            }

            Session["Mediums"] = model.Mediums;
            Session["Materials"] = model.Materials;
            Session["Styles"] = model.Styles;
            Session["Keywords"] = model.Keywords;

            Session["firstmedium"] = model.Mediums.Split(',').First();
            Session["firstmaterial"] = CashManager.Instance.Materials.SingleOrDefault(x => x.Key == model.Materials.First()).Value;
            return RedirectToAction("Setep6");
        }

        //get size with canvas
        public ActionResult Setep6()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Setep6(UploadViewModel.step6 model)
        {
            if (model.Height == 0)
            {
                ViewBag.error = Resources.UploadRes.zeroHeight_error;
                return PartialView();
            }
            if (model.Width == 0)
            {
                ViewBag.error = Resources.UploadRes.zeroWidth_error;
                return PartialView();
            }
            if (model.Depth == 0)
            {
                ViewBag.error = Resources.UploadRes.zeroDepth_error;
                return PartialView();
            }

            Session["Height"] = model.Height;
            Session["Width"] = model.Width;
            Session["Depth"] = model.Depth;

            return RedirectToAction("Setep7");
        }

        //title and description // done if nor fore sale
        public ActionResult Setep7()
        {
            ViewBag.Keywords = Session["Keywords"];
            ViewBag.firstmedium = Session["firstmedium"];
            ViewBag.firstmaterial = Session["firstmaterial"];
            return PartialView();
        }
        [HttpPost]
        public ActionResult Setep7(UploadViewModel.step7 model)
        {
            if (string.IsNullOrEmpty(model.Title))
            {
                ViewBag.error = Resources.UploadRes.titleNull_error;
                return PartialView();
            }
            if (string.IsNullOrEmpty(model.Description))
            {
                ViewBag.error = Resources.UploadRes.descriptionnull_error;
                return PartialView();
            }
            if (model.AllEntity < model.avaible)
            {
                ViewBag.error = Resources.UploadRes.avaibleEntity_error;
                return PartialView();
            }
            Session["Title"] = model.Title;
            Session["avaible"] = model.avaible;
            Session["Description"] = model.Description;
            Session["AllEntity"] = model.AllEntity;
            return RedirectToAction("Setep8");
        }

        //Packaging and frame
        public ActionResult Setep8()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Setep8(UploadViewModel.step8 model)
        {
            Session["Packaging"] = model.Packaging;
            Session["framed"] = model.framed;
            Session["multi_paneled"] = model.multi_paneled;
            return RedirectToAction("Setep9");
        }

        public ActionResult Setep9()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Setep9(UploadViewModel.step9 model)
        {

            if (string.IsNullOrEmpty(model.Street_Address) || string.IsNullOrEmpty(model.Address_2) || string.IsNullOrEmpty(model.City)
                || string.IsNullOrEmpty(model.Country) || string.IsNullOrEmpty(model.Region) || string.IsNullOrEmpty(model.Zip_code) ||
                string.IsNullOrEmpty(model.phoneNumber))
            {
                ViewBag.error = Resources.UploadRes.Empty_Error;
                return PartialView();
            }

            Session["weight"] = model.weight;
            Session["Street_Address"] = model.Street_Address;
            Session["Address_2"] = model.Address_2;
            Session["City"] = model.City;
            Session["Country"] = model.Country;
            Session["Region"] = model.Region;
            Session["Zip_code"] = model.Zip_code;
            Session["phoneNumber"] = model.phoneNumber;

            return RedirectToAction("Setep10");
        }

        public ActionResult Setep10()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Setep10(UploadViewModel.step10 model)
        {


            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var profile = user.userDetail;
            int id = 0;
            try
            {
                var widepath = (string)Session["WideFullPath"];
                var sqpath = (string)Session["SqureFullPath"];
                var categoryId = (int)Session["category"];
                var subjectId = (int)Session["subject"];
                var product = new Product()
                {
                    photo = new Photo() { Path = widepath },
                    Sqphoto = new Photo() { Path = sqpath },
                    Title = (string)Session["Title"],
                    Description = (string)Session["Description"],
                    Price = (int)model.Price,
                    ISOrginalForSale = (bool)Session["isOrginal"],
                    AllEntity = (int)Session["AllEntity"],
                    ArtCreatedYear = (int)Session["createYear"],
                    avaible = (int)Session["avaible"],
                    Depth = (float)Session["Depth"],
                    Height = (float)Session["Height"],
                    Width = (float)Session["Width"],
                    IsPrintAvaibled = false,
                    Keywords = (string)Session["Keywords"],
                    categoryId = categoryId,
                    subjectId = subjectId
                };
                profile.Products.Add(product);
                db.SaveChanges();
                id = product.Id;
            }
            catch
            {
                ViewBag.error = Resources.UploadRes.Image_cannot_be_empty;
                return PartialView();
            }
            return Json(new { result = 1313, id = id }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}