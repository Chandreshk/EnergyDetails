using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EnergyDetails
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "EnergyLevel", action = "Login", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "GoogleMap",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "EnergyLevel", action = "GoogleMap", id = UrlParameter.Optional }
            );
        }
    }
}
