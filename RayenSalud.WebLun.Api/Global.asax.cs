using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RayenSalud.WebLun.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpResponse response = app.Response;

            if (response.StatusCode != 200)
            {
                //response.SuppressContent = true;
                //response.ClearContent();
                string host = HttpContext.Current.Request.Headers["Origin"];
                if (host != null && HttpContext.Current.Response.Headers["Access-Control-Allow-Origin"] == null)
                    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", host);

                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, Authorization");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credentials", "true");
                HttpContext.Current.Response.End();
            }

        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            string host = HttpContext.Current.Request.Headers["Origin"];
            if (host != null)
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", host);
            }

            HttpApplication app = (HttpApplication)sender;
            if (app.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, Authorization");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credentials", "true");
                HttpContext.Current.Response.End();
                return;
            }
        }
    }

}
