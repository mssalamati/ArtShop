using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class ImageServersController : BaseController
    {
        [Authorize(Roles = "Superadmin")]
        public ActionResult Index()
        {
            var data = db.ImageServers.ToList();
            return View(data);
        }

        public async Task<JsonResult> getsingle(int id)
        {
            ImageServer obj = null;
            var iss = db.ImageServers.Find(id);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://" + iss.Host + "/Upload/info");
            if (response.IsSuccessStatusCode)
            {
                obj = await response.Content.ReadAsAsync<ImageServer>();
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult add()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult add(string host)
        {
            db.ImageServers.Add(new ImageServer() { Host = host });
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