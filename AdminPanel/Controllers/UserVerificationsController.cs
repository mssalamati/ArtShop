using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class UserVerificationsController : BaseController
    {
        public ActionResult Index(int page = 1)
        {
            int count = 0, pagesize = 15, take = pagesize, skip = (page - 1) * pagesize;
            var data = db.UserProfiles
                 .Where(x => string.IsNullOrEmpty(x.IdConfirmedBy) && !string.IsNullOrEmpty(x.GovermentIdPath));
            count = data.Count();
            int maxpage = count % pagesize != 0 ? (count / pagesize) + 1 : (count / pagesize);
            ViewBag.page = page; ViewBag.maxpage = maxpage;
            return View(data.OrderByDescending(x => x.RegisterDate).Skip(skip).Take(take).ToList());
        }

        public ActionResult detail(string id)
        {
            var obj = db.UserProfiles.Find(id);
            return View(obj);
        }

        public ActionResult SetConfitm(string id, bool confirm)
        {
            var obj = db.UserProfiles.Find(id);
            obj.isIDConfirmed = confirm;
            obj.IdConfirmedBy = User.Identity.Name;
            return RedirectToAction("detail", new { id = id });
        }
    }
}