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

namespace MoviesAPI.Controllers
{

    //public class Movie
    //{
    //    string Title { get; set; }
    //    string Year { get; set; }    //    string ID { get; set; }    //    string Type { get; set; }    //    string Poster { get; set; }
    //}
    public class Provider
    {
        public string Name { get; set; }
        public string GetAPI { get; set; }
        public string GetPerItemAPI { get; set; }
    }

    public class MovieSet
    {
       public string Movies { get; set; }
       public string Provider { get; set; }
    }
    public class MoviesController : ApiController
    {
        private const string ACCESS_TOKEN_VALUE = "sjd1HfkjU83ksdsm3802k";
        private const string ACCESS_TOKEN_KEY = "x-access-token";
        private const string BASS_ADDRESS = "http://webjetapitest.azurewebsites.net/";
        private const string APPLICATION_JSON = "application/json";
        private Dictionary<string, Provider> providersDictionary = new Dictionary<string, Provider>();
        public MoviesController()
        {
            providersDictionary.Add("cinemaworld", new Provider() { Name = "cinemaworld", GetAPI = "api/cinemaworld/movies", GetPerItemAPI = "api/cinemaworld/movie/{0}" });
            providersDictionary.Add("filmworld", new Provider() { Name = "filmworld", GetAPI = "api/filmworld/movies", GetPerItemAPI = "api/filmworld/movie/{0}" });
        }

        //private HttpResponseMessage CreateMovieResponse(string url)
        //{
        //    if (string.IsNullOrEmpty(url))
        //    {
        //        throw new ArgumentException("url cannot be null or empty");
        //    }

        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(BASS_ADDRESS);
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(APPLICATION_JSON));
        //        client.DefaultRequestHeaders.Add(ACCESS_TOKEN_KEY, ACCESS_TOKEN_VALUE);
        //        var response = client.GetAsync(url).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var newReponse = Request.CreateResponse(HttpStatusCode.OK);
        //            newReponse.Content = new StringContent(response.Content.ReadAsStringAsync().Result, Encoding.UTF8, APPLICATION_JSON);
        //            return newReponse;
        //        }
        //        else
        //        {
        //            return new HttpResponseMessage(response.StatusCode);
        //        }
        //    }
        //}

        private string getMovies(string url, out HttpStatusCode statusCode)
        {
            statusCode = HttpStatusCode.OK;
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("url cannot be null or empty");
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASS_ADDRESS);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(APPLICATION_JSON));
                client.DefaultRequestHeaders.Add(ACCESS_TOKEN_KEY, ACCESS_TOKEN_VALUE);
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

        // GET api/movies
        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {
                List<MovieSet> movieSets = new List<MovieSet>();
               
                foreach(Provider provider  in this.providersDictionary.Values)
                {
                    HttpStatusCode statusCode;
                    string movies = getMovies(provider.GetAPI, out statusCode);
                    if (statusCode != HttpStatusCode.OK)
                    {
                        return new HttpResponseMessage(statusCode);
                    }
                    else
                    {
                        MovieSet movieSet = new MovieSet() { Movies = movies, Provider = provider.Name };
                        movieSets.Add(movieSet);
                    }
                }

                var jsonMovieSets = new JavaScriptSerializer().Serialize(movieSets);

                var newReponse = Request.CreateResponse(HttpStatusCode.OK);
                newReponse.Content = new StringContent(jsonMovieSets, Encoding.UTF8, APPLICATION_JSON);
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
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new ArgumentException("id cannot be null or empty");
                }

                if (string.IsNullOrEmpty(provider))
                {
                    throw new ArgumentException("provider cannot be null or empty");
                }

                Provider providerObj;
                if (this.providersDictionary.TryGetValue(provider, out providerObj))
                {
                    //return CreateMovieResponse();

                    HttpStatusCode statusCode;
                    string movies = getMovies(string.Format(providerObj.GetPerItemAPI, id), out statusCode);
                    if (statusCode != HttpStatusCode.OK)
                    {
                        return new HttpResponseMessage(statusCode);
                    }
                    else
                    {
                        var newReponse = Request.CreateResponse(HttpStatusCode.OK);
                        newReponse.Content = new StringContent(movies, Encoding.UTF8, APPLICATION_JSON);
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
