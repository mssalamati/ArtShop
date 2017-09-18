using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ArtShop
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
           
            //GlobalFilters.Filters.Add(new RequireHttpsAttribute());
        }
        //protected void Application_BeginRequest()
        //{
        //    switch (Request.Url.Scheme)
        //    {
        //        case "https":
        //            Response.AddHeader("Strict-Transport-Security", "max-age=300");
        //            break;
        //        case "http":
        //            var path = "https://" + Request.Url.Host + Request.Url.PathAndQuery;
        //            Response.Status = "301 Moved Permanently";
        //            Response.AddHeader("Location", path);
        //            break;
        //    }
        //}
    }
}
