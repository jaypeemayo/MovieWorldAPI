using Microsoft.Practices.Unity;
using MoviesAPI.Controllers;
using MoviesAPI.Services;
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
            //Setup container
            var container = new UnityContainer();
            container.RegisterType<IHttpService, HttpService>(new HierarchicalLifetimeManager());
            container.RegisterType<IConfigurationService, ConfigurationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMovieServicesFactory, MovieServicesFactory>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);

            //setup CORS
            var cors = new EnableCorsAttribute("*", "*", "*"); // put * for development purpose only. In actual deployment, should set the address of client if it has different address. If the same we can remove this.
            config.EnableCors(cors);

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
