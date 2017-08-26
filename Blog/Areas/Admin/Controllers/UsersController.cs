using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Blog.Areas.Admin.Models.ViewModel;

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
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.ApplicationUserDetail.Email,
                    RegisterDate = x.RegisterDate.ToString()
                }).ToList();

            int maxpage = count % pagesize != 0 ? (count / pagesize) + 1 : (count / pagesize);
            ViewBag.page = page;
            ViewBag.maxpage = maxpage;
            ViewBag.search = search;
            return View(outres);
        }
    }
}