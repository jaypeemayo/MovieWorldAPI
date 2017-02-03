using MoviesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace MoviesAPI.Services
{
    public class WebJetWrapperService : IWebJetWrapperService
    {
        IConfigurationService iConfigurationService;
        public WebJetWrapperService(IConfigurationService iConfigurationService)
        {
            this.iConfigurationService = iConfigurationService;
        }

        public string GetMovies(string url, out HttpStatusCode statusCode)
        {
            statusCode = HttpStatusCode.OK;
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("url cannot be null or empty");
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(iConfigurationService.BaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.APPLICATION_JSON));
                client.DefaultRequestHeaders.Add(Constants.ACCESS_TOKEN_KEY, iConfigurationService.AccessToken);
                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    statusCode = response.StatusCode;
                }
            }

            return null;
        }
    }
}