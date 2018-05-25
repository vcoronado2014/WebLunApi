using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Headers;

namespace RayenSalud.WebLun.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de API web

            // Rutas de API web
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));


            config.MapHttpAttributeRoutes();

            #region  Login
            config.Routes.MapHttpRoute(
                name: "Login",
                routeTemplate: "api/Login",
                defaults: new
                {
                    controller = "Login"
                }
            );
            #endregion

            #region  EntidadContratante
            config.Routes.MapHttpRoute(
                name: "EntidadContratante",
                routeTemplate: "api/EntidadContratante",
                defaults: new
                {
                    controller = "EntidadContratante"
                }
            );
            #endregion

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
