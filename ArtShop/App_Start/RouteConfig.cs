﻿using ArtShop.Helper;
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

            routes.Add("ProductDetails", new SeoFriendlyRoute("{culture}/Artwork/{details}/{id}",
            new RouteValueDictionary(new { controller = "Products", culture = string.Empty, action = "single" }),
            new MvcRouteHandler()));

            routes.Add("Articles", new SeoFriendlyRoute("{culture}/Support/Article/{id}",
            new RouteValueDictionary(new { controller = "Support", culture = string.Empty, action = "Article" }),
            new MvcRouteHandler()));

            routes.Add("ArticlesModified", new SeoFriendlyRoute("Support/Article/{id}",
            new RouteValueDictionary(new { controller = "Support", action = "Article" }),
            new MvcRouteHandler()));

            routes.Add("SubCategory", new SeoFriendlyRoute("{culture}/Support/SubCategory/{id}",
            new RouteValueDictionary(new { controller = "Support", culture = string.Empty, action = "SubCategory" }),
            new MvcRouteHandler()));

            routes.Add("Category", new SeoFriendlyRoute("{culture}/Support/Category/{id}",
            new RouteValueDictionary(new { controller = "Support", culture = string.Empty, action = "Category" }),
            new MvcRouteHandler()));

            routes.MapRoute(
             name: "Search",
             url: "{culture}/Search/{id}",
             defaults: new { controller = "Products", action = "Search", culture = string.Empty, id = UrlParameter.Optional }
         );

            routes.MapRoute(
                name: "DefaultWithCulture",
                url: "{culture}/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", culture = string.Empty, id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           );
        }
    }
}
