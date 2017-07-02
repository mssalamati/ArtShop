using ArtShop.Models;
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

namespace ArtShop.Controllers
{
    public class UploadController : Controller
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

        public ActionResult Setep1()
        {
            ViewBag.lastpic = Session["imageAddress"];
            return PartialView();
        }
        [HttpPost]
        public ActionResult Setep1(UploadViewModel.step1 model)
        {
            Session["imageAddress"] = model.img;
            if (string.IsNullOrEmpty(model.img) || !System.IO.File.Exists(HttpContext.Server.MapPath(model.img)))
            {
                ViewBag.Error = "Image cannot be empty";
                return PartialView();
            }
            return RedirectToAction("Setep2");
        }

        public ActionResult Setep2()
        {
            ViewBag.subjects = CashManager.Instance.Subjects;
            ViewBag.category = CashManager.Instance.Categories;
            return PartialView();
        }
        [HttpPost]
        public ActionResult Setep2(UploadViewModel.step2 model)
        {
            Session["category"] = model.category;
            Session["subject"] = model.subject;
            return RedirectToAction("Setep3");
        }

        public ActionResult Setep3()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Setep3(UploadViewModel.step3 model)
        {
            Session["copyright"] = model.copyright;
            Session["createYear"] = model.createYear;
            Session["isOrginal"] = model.isOrginal;
            Session["printAvable"] = model.printAvable;
            return RedirectToAction("Setep4");
        }

        public ActionResult Setep4()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Setep4(UploadViewModel.step4 model)
        {
            return RedirectToAction("Setep5");
        }

        public ActionResult Setep5()
        {
            ViewBag.lastpic = Session["imageAddress"];
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
                return PartialView();
            }
               
            Session["Keywords"] = model.Keywords;
            Session["firstmedium"] = model.Mediums.Split(',').First();
            Session["firstmaterial"] = CashManager.Instance.Materials.SingleOrDefault(x => x.Key == model.Materials.First()).Value;
            return RedirectToAction("Setep6");
        }

        public ActionResult Setep6()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Setep6(UploadViewModel.step6 model)
        {
            return RedirectToAction("Setep7");
        }

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
           
            return RedirectToAction("Setep8");
        }

        public ActionResult Setep8()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Setep8(UploadViewModel.step8 model)
        {
            return RedirectToAction("Setep9");
        }

        public ActionResult Setep9()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Setep9(UploadViewModel.step9 model)
        {
            return RedirectToAction("Setep10");
        }

        public ActionResult Setep10()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Setep10(UploadViewModel.step10 model)
        {
            return RedirectToAction("Setep11");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}