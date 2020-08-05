using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CrudDemo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Route to handle Create New operation
            routes.MapRoute(
                name: "Create",
                url: "user/create",
                defaults: new { controller = "User", action = "Create" }
            );

            // Route to handle Edit/Delete  and without default Index for listing data
            routes.MapRoute(
                name: "User",
                url: "user/{id}",
                defaults: new { controller = "User", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
