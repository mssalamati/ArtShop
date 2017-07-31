using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class ImageServersController : BaseController
    {
        public ActionResult Index()
        {
            var data = db.ImageServers.ToList();
            foreach (var item in data)
            {

            }
            return View(data);
        }
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult add()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult add(string host)
        {


            db.SaveChanges();
            return PartialView("_successWindow");
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var finder = db.ImageServers.Find(id);
            db.ImageServers.Remove(finder);
            db.SaveChanges();
            return RedirectToAction("index");
        }
    }
}