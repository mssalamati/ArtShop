using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class PayoutsController : BaseController
    {
        public ActionResult Index(int page = 1)
        {
            int count = 0, pagesize = 15, take = pagesize, skip = (page - 1) * pagesize;
            var data = db.PayoutRequests;
            count = data.Count();
            int maxpage = count % pagesize != 0 ? (count / pagesize) + 1 : (count / pagesize);
            ViewBag.page = page; ViewBag.maxpage = maxpage;
            return View(data.OrderByDescending(x => x.date).Skip(skip).Take(take).ToList());
        }

        public ActionResult detail(int id)
        {
            var obj = db.PayoutRequests.Find(id);
            return View(obj);
        }

        public ActionResult setPayStatus(int id, bool payed)
        {
            var obj = db.PayoutRequests.Find(id);
            obj.Seen = true;
            obj.Payed = payed;
            db.SaveChanges();
            return RedirectToAction("detail", new { id = id });
        }
    }
}