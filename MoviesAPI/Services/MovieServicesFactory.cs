using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoviesAPI.Services
{
    public class MovieServicesFactory : IMovieServicesFactory
    {
        private IUnityContainer container;
        public MovieServicesFactory(IUnityContainer container)
        {
            this.container = container;
        }

        public IConfigurationService GetConfigurationService()
        {
            return this.container.Resolve(typeof(IConfigurationService)) as IConfigurationService;
        }

        public IWebJetWrapperService GetWebJetWrapperService()
        {
            return this.container.Resolve(typeof(WebJetWrapperService)) as IWebJetWrapperService;
        }
    }
}