using ArtShop.Models;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

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
            return RedirectToAction("Setep2");
        }

        public ActionResult Setep2()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Setep2(UploadViewModel.step2 model)
        {
            return RedirectToAction("Setep3");
        }

        public ActionResult Setep3()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Setep3(UploadViewModel.step3 model)
        {
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
            return PartialView();
        }
        [HttpPost]
        public ActionResult Setep5(UploadViewModel.step5 model)
        {
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