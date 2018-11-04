using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    [Authorize(Roles = "Superadmin,Administrator")]
    public class SettingController : BaseController
    {
        public ActionResult Index()
        {
            return View(db.SettingValues.FirstOrDefault());
        }

        public ActionResult edit(int id)
        {
            var finder = db.SettingValues.FirstOrDefault();
            return PartialView(finder);
        }

        [HttpPost]
        public ActionResult edit(SettingValue model)
        {
            var finder = db.SettingValues.FirstOrDefault();
            try
            {
                finder.IRRialRate = model.IRRialRate;
                finder.EURate = model.EURate;
                finder.UpdateDate = DateTime.Now;
                finder.UpdaterUser = User.Identity.Name;
                db.SaveChanges();
                return PartialView("_successWindow");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.ToString());
            }
            return PartialView(finder);
        }
    }
}