using MoviesAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MoviesAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

    //        config.Formatters.JsonFormatter.SupportedMediaTypes
    //.Add(new MediaTypeHeaderValue("text/html"));
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}/{provider}",
                defaults: new { id = RouteParameter.Optional , provider = RouteParameter.Optional}
            );
        }
    }
}
