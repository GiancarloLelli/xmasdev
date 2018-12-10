using System.Web.Mvc;
using System.Web.Routing;

namespace XmasDev.Feedback
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{user}/{product}",
                defaults: new { controller = "Home", action = "Index", user = UrlParameter.Optional, product = UrlParameter.Optional }
            );
        }
    }
}
