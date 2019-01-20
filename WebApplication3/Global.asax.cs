using Logik;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebApplication3
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new DataContextInitializer());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/js").Include("~/Scripts/jquery-{version}.js",
                "~/Scripts/bootstrap.js", "~/Scripts/respond.js"));
            BundleTable.Bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.css",
               "~/Content/Site.css"));
        }
    }
}
