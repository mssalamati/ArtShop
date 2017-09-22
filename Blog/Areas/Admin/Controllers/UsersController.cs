using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Blog.Areas.Admin.Models.ViewModel;
using Blog.Models;
using System.Threading.Tasks;

namespace Blog.Areas.Admin.Controllers
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

            if (User.Identity.GetUserName() != "admin")
                data = data.Where(x => x.ApplicationUserDetail.UserName != "admin");

            data = data.OrderByDescending(x => x.RegisterDate);
            count = data.Count();
            var outres = data.Skip(skip).Take(take).ToList()
                .Select(x => new UserViewModel()
                {
                    id = x.Id,
                    FullName = x.FullName,
                    Username = x.ApplicationUserDetail.UserName,
                    Email = x.ApplicationUserDetail.Email,
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
                var userDetail = new UserProfile { FullName = model.Fullname, profileType = model.profileType };
                var user = new ApplicationUser { UserName = model.Username, Email = model.Email, userDetail = userDetail };
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
                Fullname = user.userDetail.FullName,
                profileType = ProfileType.Admin,
                Username = user.UserName
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
                user.userDetail.FullName = model.Fullname;
                user.UserName = model.Username;

                if (!string.IsNullOrEmpty(model.NewPassword))
                {
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

        public ActionResult Deactive(string id)
        {


            return RedirectToAction("Index");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}