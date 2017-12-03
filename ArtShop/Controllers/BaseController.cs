using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading;
using ArtShop.Helper;

namespace ArtShop.Controllers
{
    public class BaseController : Controller
    {
        protected ApplicationDbContext db = new ApplicationDbContext();

        protected ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            //string cultureName = null;

            //// Attempt to read the culture cookie from Request
            //HttpCookie cultureCookie = Request.Cookies["_culture"];
            //if (cultureCookie != null)
            //    cultureName = cultureCookie.Value;
            //else
            //    cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
            //            Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
            //            null;
            //// Validate culture name
            //cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

            //// Modify current thread's cultures            
            //Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            //Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            //return base.BeginExecuteCore(callback, state);

            string cultureName = RouteData.Values["culture"] as string;

            // Attempt to read the culture cookie from Request
            if (cultureName == null)
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : null; // obtain it from HTTP header AcceptLanguages

            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe


            //if (RouteData.Values["culture"] as string != cultureName.ToLower())
            //{
            //    // Force a valid culture in the URL
            //    RouteData.Values["culture"] = cultureName.ToLowerInvariant(); // lower case too

            //    if (RouteData.Values.Values.Any(a => a.ToString().ToLower().Contains("article")))
            //    {
            //        if (RouteData.Values.Values.Any(a => a.ToString().ToLower().Contains("en-us")))
            //        {
            //            Response.RedirectToRoute("Articles");
            //        }

            //        else
            //            Response.RedirectToRoute("ArticlesModified");
            //    }
            //    else if (RouteData.Values.Values.Any(a => a.ToString().ToLower().Contains("products")))
            //    {
            //        Response.RedirectToRoute("Search");
            //    }
            //    else
            //        //Redirect user
            //        Response.RedirectToRoute("DefaultWithCulture");
            //}


            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;


            return base.BeginExecuteCore(callback, state);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}