using MoviesAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoviesAPI.Services
{
    public class ConfigurationService:IConfigurationService
    {
        private IDictionary<string, IProvider> providersDictionary = new Dictionary<string, IProvider>();
        private string accessToken;
        private string baseAddress;

        public ConfigurationService()
        {
            string mykey = System.Configuration.ConfigurationManager.AppSettings["PROVIDERS"];
            IProvider[] results = JsonConvert.DeserializeObject<Provider[]>(mykey);
            foreach (IProvider result in results)
            {
                providersDictionary.Add(result.Name, result);
            }
            accessToken = System.Configuration.ConfigurationManager.AppSettings["ACCESS_TOKEN_VALUE"];
            baseAddress = System.Configuration.ConfigurationManager.AppSettings["BASE_ADDRESS"];
        }

        public string AccessToken
        {
            get
            {
                return this.accessToken;
            }
        }

        public string BaseAddress
        {
            get
            {
                return this.baseAddress;
            }
        }

        public IDictionary<string, IProvider> ProvidersDictionary
        {
            get
            {
                return this.providersDictionary;
            }
        }
    }
}