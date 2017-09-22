using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class OrdersController : BaseController
    {
        public ActionResult Index(int page = 1)
        {
            int count = 0, pagesize = 15, take = pagesize, skip = (page - 1) * pagesize;
            var data = db.Orders
                 .OrderByDescending(x => x.BuyDate)
                 .Skip(skip).Take(take);
            count = data.Count();
            int maxpage = count % pagesize != 0 ? (count / pagesize) + 1 : (count / pagesize);
            ViewBag.page = page; ViewBag.maxpage = maxpage;
            return View(data.ToList());
        }

        public ActionResult detail(int id)
        {
            var obj = db.Orders.Find(id);
            return View(obj);
        }

        public ActionResult setStatus(int id, OrderStatus status)
        {
            var obj = db.Orders.Find(id);
            obj.Status = status;
            db.SaveChanges();
            return RedirectToAction("detail", new { id = id });
        }
    }
}