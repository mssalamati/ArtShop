using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ArtShop.Models;
using System.Threading.Tasks;
using DataLayer.Enitities;

namespace ArtShop.Controllers
{
    public class AccountInfoController : BaseController
    {
        // GET: AccountInfo
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var userProfile = db.UserProfiles.Find(userId);

            AccountInfoViewModel model = new AccountInfoViewModel();
            model.FirstName = userProfile.FirstName;
            model.LastName = userProfile.LastName;
            model.Email = userProfile.ApplicationUserDetail.Email;
            model.profileType = userProfile.profileType;
            model.ReceiveNewArtEmail = userProfile.ReceiveNewArtEmail;
            model.MailingList = userProfile.MailingList;

            return View(model);
        }
    }
}