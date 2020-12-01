using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace UygulamaOdevi2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Login",
                url: "{Login}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "SignUp",
                url: "{SignUp}",
                defaults: new { controller = "SignUp", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CreateConference",
                url: "{CreateConference}",
                defaults: new { controller = "CreateConference", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
