using DataLayer.Enitities;
using Postal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    [Authorize(Roles = "Superadmin,Administrator,Manager")]
    public class UserVerificationsController : BaseController
    {
        public ActionResult Index(int page = 1)
        {
            int count = 0, pagesize = 15, take = pagesize, skip = (page - 1) * pagesize;
            var data = db.UserProfiles.Where(x => !string.IsNullOrEmpty(x.GovermentIdPath))
                  .OrderByDescending(x => string.IsNullOrEmpty(x.IdConfirmedBy));
            count = data.Count();
            int maxpage = count % pagesize != 0 ? (count / pagesize) + 1 : (count / pagesize);
            ViewBag.page = page; ViewBag.maxpage = maxpage;
            return View(data.Skip(skip).Take(take).ToList());
        }

        public ActionResult detail(string id)
        {
            var obj = db.UserProfiles.Find(id);
            return View(obj);
        }

        public ActionResult SetConfitm(string id, bool confirm, string Reason)
        {
            var obj = db.UserProfiles.Find(id);
            obj.isIDConfirmed = confirm;
            obj.IdConfirmedBy = User.Identity.Name;


            if (confirm)
            {
                obj.IDStatus = IDCardStatus.Confirmed;
                var artworks = obj.Products.Where(a => a.Status != ProductStatus.Sold && a.avaible >= 1 && a.Price > 0);
                foreach (var item in artworks)
                {
                    item.Status = ProductStatus.forSale;
                }
            }
            else
            {
                obj.IDStatus = IDCardStatus.NotConfirmed;
                obj.IdRejectionReason = Reason;
            }
            db.SaveChanges();
            SendEmail(obj);
            return RedirectToAction("detail", new { id = id });
        }
        private void SendEmail(UserProfile userProfile)
        {
            dynamic email = new Email("IdVerification");
            email.To = userProfile.ApplicationUserDetail.UserName;
            email.Subject = "Id Verification Status";
            email.Status = userProfile.isIDConfirmed;
            email.Send();
        }
    }
}