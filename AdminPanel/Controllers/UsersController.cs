using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using AdminPanel.Models.ViewModel;
using DataLayer.Enitities;
using System.Threading.Tasks;
using AdminPanel.Models;
using DataLayer;

namespace AdminPanel.Controllers
{
    public class UsersController : BaseController
    {
        public ActionResult Index(int page = 1, string search = "")
        {
            int count = 0;
            int pagesize = 15;
            int take = pagesize;
            int skip = (page - 1) * pagesize;

            var roles = db.Roles.ToDictionary(x => x.Id, x => x.Name == "superadmin" ? "SA" : x.Name == "admin" ? "A" : x.Name == "agency" ? "B" : "C");

            var data = db.UserProfiles.Where(x => string.IsNullOrEmpty(search) || x.ApplicationUserDetail.UserName.Contains(search)).AsQueryable();

            if (User.Identity.GetUserName() != "superadmin")
                data = data.Where(x => x.ApplicationUserDetail.UserName != "superadmin");

            data = data.OrderByDescending(x => x.RegisterDate);
            count = data.Count();
            var outres = data.Skip(skip).Take(take).ToList()
                .Select(x => new UserViewModel()
                {
                    id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.ApplicationUserDetail.Email,
                    Username = x.ApplicationUserDetail.UserName,
                    RegisterDate = x.RegisterDate.ToString()
                }).ToList();

            int maxpage = count % pagesize != 0 ? (count / pagesize) + 1 : (count / pagesize);
            ViewBag.page = page;
            ViewBag.maxpage = maxpage;
            ViewBag.search = search;
            return View(outres);
        }

        public ActionResult Add()
        {
            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string prefix = "sys.";
                string postfix = "@artiscovery.com";
                string fullEmail = prefix + model.FirstName + model.LastName + postfix;
                var userDetail = new UserProfile { FirstName = model.FirstName, LastName = model.LastName, profileType = ProfileType.Artist, MailingList = true };
                var user = new ApplicationUser { UserName = fullEmail, Email = fullEmail, userDetail = userDetail,EmailConfirmed = true };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    return PartialView("_successWindow");
                }

                AddErrors(result);
                return PartialView(model);
            }

            return PartialView(model);
        }

        public ActionResult Edit(string id)
        {
            var user = db.Users.Find(id);
            var data = new EditUserViewModel
            {
                id = user.Id,
                Email = user.Email,
                FirstName = user.userDetail.FirstName,
                LastName = user.userDetail.LastName,
                profileType = ProfileType.Artist,
            };

            return PartialView(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserViewModel model)
        {
            var user = db.Users.Find(model.id);

            if (ModelState.IsValid)
            {
                user.Email = model.Email;
                user.userDetail.FirstName = model.FirstName;
                user.userDetail.LastName = model.LastName;

                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    await UserManager.RemovePasswordAsync(model.id);
                    var result = await UserManager.AddPasswordAsync(model.id, model.NewPassword);

                    if (result.Succeeded)
                    {
                        db.SaveChanges();
                        return PartialView("_successWindow");
                    }

                    AddErrors(result);
                    return PartialView(model);

                }

                db.SaveChanges();
            }

            return PartialView("_successWindow");
        }

        //public ActionResult Login(LoginViewModel model)
        //{

        //    var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(returnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.Failure:
        //        default:
        //            ModelState.AddModelError("", "Invalid login attempt.");
        //            return View(model);
        //    }


        //}

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}