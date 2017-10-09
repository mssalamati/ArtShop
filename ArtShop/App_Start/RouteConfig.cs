using ArtShop.Helper;
using Canonicalize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ArtShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            routes.Canonicalize().NoWww().Lowercase().NoTrailingSlash();
            routes.Add("ProductDetails", new SeoFriendlyRoute("Artwork/{details}/{id}",
       new RouteValueDictionary(new { controller = "Products", action = "single" }),
       new MvcRouteHandler()));

            routes.Add("Articles", new SeoFriendlyRoute("Support/{Article}/{id}",
new RouteValueDictionary(new { controller = "Support", action = "Article" }),
new MvcRouteHandler()));

            routes.Add("SubCategory", new SeoFriendlyRoute("Support/{SubCategory}/{id}",
new RouteValueDictionary(new { controller = "Support", action = "SubCategory" }),
new MvcRouteHandler()));

            routes.Add("Category", new SeoFriendlyRoute("Support/{Category}/{id}",
new RouteValueDictionary(new { controller = "Support", action = "Category" }),
new MvcRouteHandler()));

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
