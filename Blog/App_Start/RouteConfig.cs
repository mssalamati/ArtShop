﻿using Blog.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Blog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Add("SubCategory", new SeoFriendlyRoute("Category/{Index}/{id}",
          new RouteValueDictionary(new { controller = "Category", action = "SubCategory", Index = UrlParameter.Optional }),
          new MvcRouteHandler()));

            routes.Add("Category", new SeoFriendlyRoute("Category/{id}",
          new RouteValueDictionary(new { controller = "Category", action = "Index", Index = UrlParameter.Optional }),
          new MvcRouteHandler()));

            routes.MapRoute(
        "item_details",
        "search/{id}",
        new { controller = "Search", action = "Index" }
        );

            routes.MapRoute(
          "tag_details",
          "Tag/{id}",
          new { controller = "Tag", action = "Index" }
           );

            routes.Add("PostDetails", new SeoFriendlyRoute("post/{Index}/{id}",
                new RouteValueDictionary(new { controller = "post", action = "Index", Index = UrlParameter.Optional }),
                new MvcRouteHandler()));

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
              namespaces: new[] { "Blog.Controllers" }
            );


            foreach (var route in routes.Cast<Route>().Where(route =>
            route.GetType() == typeof(SeoFriendlyRoute) || route.GetType() == typeof(Route)))
            {
                if (!route.Url.Contains("Admin"))
                {
                    route.Url = "{culture}/" + route.Url;
                    route.Defaults.Add("culture", String.Empty);

                }
            }
        }

    }
}
