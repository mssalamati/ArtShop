using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtShop.Controllers
{
    public class UploadController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Setep1()
        {
            return PartialView();
        }
        public ActionResult Setep2()
        {
            return PartialView();
        }
        public ActionResult Setep3()
        {
            return PartialView();
        }
        public ActionResult Setep4()
        {
            return PartialView();
        }
        public ActionResult Setep5()
        {
            return PartialView();
        }
        public ActionResult Setep6()
        {
            return PartialView();
        }
        public ActionResult Setep7()
        {
            return PartialView();
        }
        public ActionResult Setep8()
        {
            return PartialView();
        }

        public ActionResult Setep9()
        {
            return PartialView();
        }

        public ActionResult Setep10()
        {
            return PartialView();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}