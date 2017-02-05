using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using MoviesAPI.Models;
using MoviesAPI.Services;
using System.IO;
using System.Drawing;

namespace MoviesAPI.Controllers
{
    public class MoviesController : ApiController
    {
        IHttpService httpService;
        IConfigurationService configurationService;
        private IDictionary<string, IProvider> providersDictionary;

        public MoviesController(IMovieServicesFactory movieServicesFactory)
        {
            this.httpService = movieServicesFactory.GetHttpService();
            this.configurationService = movieServicesFactory.GetConfigurationService();
            this.providersDictionary = configurationService.ProvidersDictionary;
        }

        // GET api/movies
        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {
                List<IMovieSet> movieSets = new List<IMovieSet>();

                foreach (IProvider provider in this.providersDictionary.Values)
                {
                    HttpStatusCode statusCode;
                    string movies = httpService.Get(provider.GetAPI, out statusCode);
                    if (statusCode != HttpStatusCode.OK)
                    {
                        return new HttpResponseMessage(statusCode);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(movies))
                        {
                            MovieSet movieSet = new MovieSet() { Movies = movies, Provider = provider.Name };
                            movieSets.Add(movieSet);
                        }
                    }
                }

                var jsonMovieSets = new JavaScriptSerializer().Serialize(movieSets);

                var newReponse = Request.CreateResponse(HttpStatusCode.OK);
                newReponse.Content = new StringContent(jsonMovieSets, Encoding.UTF8, Constants.APPLICATION_JSON);
                return newReponse;
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // GET api/movies/5/cinemaworld
        public HttpResponseMessage Get(string id, string provider)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("id cannot be null or empty");
            }

            if (string.IsNullOrEmpty(provider))
            {
                throw new ArgumentException("provider cannot be null or empty");
            }

            try
            {           
                IProvider providerObj;
                if (this.providersDictionary.TryGetValue(provider, out providerObj))
                {
                    HttpStatusCode statusCode;
                    string movies = httpService.Get(string.Format(providerObj.GetPerItemAPI, id), out statusCode);
                    if (statusCode != HttpStatusCode.OK)
                    {
                        return new HttpResponseMessage(statusCode);
                    }
                    else
                    {
                        var newReponse = Request.CreateResponse(HttpStatusCode.OK);
                        newReponse.Content = new StringContent(movies, Encoding.UTF8, Constants.APPLICATION_JSON);
                        return newReponse;
                    }
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }         
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

    }
}
