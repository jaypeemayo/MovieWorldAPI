using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoviesAPI.Controllers;
using Moq;
using MoviesAPI.Services;
using MoviesAPI.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http;
using Newtonsoft.Json;

namespace MovieAPIUnitTest
{
    [TestClass]
    public class MovieControllerTest
    {
        [TestMethod]
        public void TestGet_ReturnServiceUnavailable()
        {

            string movieListString = "[{\"Title\": \"Star Wars: Episode IV - A New Hope\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}]";
            string movieListString2 = "[{\"Title\": \"Star Wars: Episode IV - A New Hope 2\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}]";


            Mock<IMovieServicesFactory> movieServicesFactoryMock = new Mock<IMovieServicesFactory>();
            Mock<IConfigurationService> configurationServiceMock = new Mock<IConfigurationService>();

            IDictionary<string, IProvider> providersDictionary = new Dictionary<string, IProvider>();
            providersDictionary.Add("cinemaworld", new Provider() { Name = "cinemaworld", GetAPI = "GetAPI", GetPerItemAPI = "GetPerItemAPI" });
            providersDictionary.Add("filmworld", new Provider() { Name = "filmworld", GetAPI = "GetAPI2", GetPerItemAPI = "GetPerItemAPI2" });
            configurationServiceMock.Setup(o => o.ProvidersDictionary).Returns(providersDictionary);

            Mock<IWebJetWrapperService> webJetWrapperServiceMock = new Mock<IWebJetWrapperService>();
            HttpStatusCode httpStatusCode = HttpStatusCode.ServiceUnavailable;
            webJetWrapperServiceMock.Setup(o => o.GetMovies("GetAPI", out httpStatusCode)).Returns(movieListString);
            webJetWrapperServiceMock.Setup(o => o.GetMovies("GetAPI2", out httpStatusCode)).Returns(movieListString2);
            movieServicesFactoryMock.Setup(o => o.GetConfigurationService()).Returns(configurationServiceMock.Object);
            movieServicesFactoryMock.Setup(o => o.GetWebJetWrapperService()).Returns(webJetWrapperServiceMock.Object);


            MoviesController movieController = new MoviesController(movieServicesFactoryMock.Object);

            movieController.Request = new HttpRequestMessage();
            movieController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            HttpResponseMessage responseMessage = movieController.Get();
            Assert.AreEqual(responseMessage.StatusCode, HttpStatusCode.ServiceUnavailable); 

        }

        [TestMethod]
        public void TestGet_ReturnInternalServerError()
        {

            string movieListString = "[{\"Title\": \"Star Wars: Episode IV - A New Hope\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}]";
            string movieListString2 = "[{\"Title\": \"Star Wars: Episode IV - A New Hope 2\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}]";


            Mock<IMovieServicesFactory> movieServicesFactoryMock = new Mock<IMovieServicesFactory>();
            Mock<IConfigurationService> configurationServiceMock = new Mock<IConfigurationService>();

            IDictionary<string, IProvider> providersDictionary = new Dictionary<string, IProvider>();
            providersDictionary.Add("cinemaworld", new Provider() { Name = "cinemaworld", GetAPI = "GetAPI", GetPerItemAPI = "GetPerItemAPI" });
            providersDictionary.Add("filmworld", new Provider() { Name = "filmworld", GetAPI = "GetAPI2", GetPerItemAPI = "GetPerItemAPI2" });
            configurationServiceMock.Setup(o => o.ProvidersDictionary).Returns(providersDictionary);

            Mock<IWebJetWrapperService> webJetWrapperServiceMock = new Mock<IWebJetWrapperService>();
            HttpStatusCode httpStatusCode = HttpStatusCode.ServiceUnavailable;
            webJetWrapperServiceMock.Setup(o => o.GetMovies("GetAPI", out httpStatusCode)).Throws(new InvalidOperationException());
            webJetWrapperServiceMock.Setup(o => o.GetMovies("GetAPI2", out httpStatusCode)).Returns(movieListString2);
            movieServicesFactoryMock.Setup(o => o.GetConfigurationService()).Returns(configurationServiceMock.Object);
            movieServicesFactoryMock.Setup(o => o.GetWebJetWrapperService()).Returns(webJetWrapperServiceMock.Object);


            MoviesController movieController = new MoviesController(movieServicesFactoryMock.Object);

            movieController.Request = new HttpRequestMessage();
            movieController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            HttpResponseMessage responseMessage = movieController.Get();
            Assert.AreEqual(responseMessage.StatusCode, HttpStatusCode.InternalServerError);

        }

        [TestMethod]
        public void TestGet_ReturnMovieSets()
        {
            string movieListString = "[{\"Title\": \"Star Wars: Episode IV - A New Hope\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}]";
            string movieListString2 = "[{\"Title\": \"Star Wars: Episode IV - A New Hope 2\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}]";


            Mock<IMovieServicesFactory> movieServicesFactoryMock = new Mock<IMovieServicesFactory>();
            Mock<IConfigurationService> configurationServiceMock = new Mock<IConfigurationService>();

            IDictionary<string, IProvider> providersDictionary = new Dictionary<string, IProvider>();
            providersDictionary.Add("cinemaworld", new Provider() { Name = "cinemaworld", GetAPI = "GetAPI", GetPerItemAPI = "GetPerItemAPI" });
            providersDictionary.Add("filmworld", new Provider() { Name = "filmworld", GetAPI = "GetAPI2", GetPerItemAPI = "GetPerItemAPI2" });
            configurationServiceMock.Setup(o => o.ProvidersDictionary).Returns(providersDictionary);

            Mock<IWebJetWrapperService> webJetWrapperServiceMock = new Mock<IWebJetWrapperService>();
            HttpStatusCode httpStatusCode = HttpStatusCode.OK;
            webJetWrapperServiceMock.Setup(o => o.GetMovies("GetAPI", out httpStatusCode)).Returns(movieListString);
            webJetWrapperServiceMock.Setup(o => o.GetMovies("GetAPI2", out httpStatusCode)).Returns(movieListString2);
            movieServicesFactoryMock.Setup(o => o.GetConfigurationService()).Returns(configurationServiceMock.Object);
            movieServicesFactoryMock.Setup(o => o.GetWebJetWrapperService()).Returns(webJetWrapperServiceMock.Object);

  
            MoviesController movieController = new MoviesController(movieServicesFactoryMock.Object);

            movieController.Request = new HttpRequestMessage();
            movieController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            HttpResponseMessage responseMessage =  movieController.Get();
            if (responseMessage.IsSuccessStatusCode)
            {
            
                string responseString = responseMessage.Content.ReadAsStringAsync().Result;
                MovieSet[] results = JsonConvert.DeserializeObject<MovieSet[]>(responseString);
                Assert.AreEqual(results[0].Movies, movieListString);
                Assert.AreEqual(results[0].Provider, "cinemaworld");
                Assert.AreEqual(results[1].Movies, movieListString2);
                Assert.AreEqual(results[1].Provider, "filmworld");
            }
        }

        [TestMethod]
        public void TestGetPerMovie_ReturnMovie()
        {
            string movieListString = "[{\"Title\": \"Star Wars: Episode IV - A New Hope\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}]";
            string movieListString2 = "[{\"Title\": \"Star Wars: Episode IV - A New Hope 2\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}]";

            string movie = "{\"Title\": \"Star Wars: Episode IV - A New Hope\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}";
            string movie2 = "{\"Title\": \"Star Wars: Episode IV - A New Hope2\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}";

            Mock<IMovieServicesFactory> movieServicesFactoryMock = new Mock<IMovieServicesFactory>();
            Mock<IConfigurationService> configurationServiceMock = new Mock<IConfigurationService>();

            IDictionary<string, IProvider> providersDictionary = new Dictionary<string, IProvider>();
            providersDictionary.Add("cinemaworld", new Provider() { Name = "cinemaworld", GetAPI = "GetAPI", GetPerItemAPI = "GetPerItemAPI" });
            providersDictionary.Add("filmworld", new Provider() { Name = "filmworld", GetAPI = "GetAPI2", GetPerItemAPI = "GetPerItemAPI2" });
            configurationServiceMock.Setup(o => o.ProvidersDictionary).Returns(providersDictionary);

            Mock<IWebJetWrapperService> webJetWrapperServiceMock = new Mock<IWebJetWrapperService>();
            HttpStatusCode httpStatusCode = HttpStatusCode.OK;
            webJetWrapperServiceMock.Setup(o => o.GetMovies("GetPerItemAPI", out httpStatusCode)).Returns(movie);
            webJetWrapperServiceMock.Setup(o => o.GetMovies("GetPerItemAPI", out httpStatusCode)).Returns(movie2);
            movieServicesFactoryMock.Setup(o => o.GetConfigurationService()).Returns(configurationServiceMock.Object);
            movieServicesFactoryMock.Setup(o => o.GetWebJetWrapperService()).Returns(webJetWrapperServiceMock.Object);

            MoviesController movieController = new MoviesController(movieServicesFactoryMock.Object);

            movieController.Request = new HttpRequestMessage();
            movieController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            HttpResponseMessage responseMessage = movieController.Get("cw0076759", "cinemaworld");
            if (responseMessage.IsSuccessStatusCode)
            {
                string responseString = responseMessage.Content.ReadAsStringAsync().Result;      
                Assert.AreEqual(responseString, movie2);
            }
        }

        [TestMethod]
        public void TestGetPerMovie_ReturnInternalServerError()
        {
            string movieListString = "[{\"Title\": \"Star Wars: Episode IV - A New Hope\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}]";
            string movieListString2 = "[{\"Title\": \"Star Wars: Episode IV - A New Hope 2\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}]";

            string movie = "{\"Title\": \"Star Wars: Episode IV - A New Hope\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}";
            string movie2 = "{\"Title\": \"Star Wars: Episode IV - A New Hope2\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}";

            Mock<IMovieServicesFactory> movieServicesFactoryMock = new Mock<IMovieServicesFactory>();
            Mock<IConfigurationService> configurationServiceMock = new Mock<IConfigurationService>();

            IDictionary<string, IProvider> providersDictionary = new Dictionary<string, IProvider>();
            providersDictionary.Add("cinemaworld", new Provider() { Name = "cinemaworld", GetAPI = "GetAPI", GetPerItemAPI = "GetPerItemAPI" });
            providersDictionary.Add("filmworld", new Provider() { Name = "filmworld", GetAPI = "GetAPI2", GetPerItemAPI = "GetPerItemAPI2" });
            configurationServiceMock.Setup(o => o.ProvidersDictionary).Returns(providersDictionary);

            Mock<IWebJetWrapperService> webJetWrapperServiceMock = new Mock<IWebJetWrapperService>();
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
            webJetWrapperServiceMock.Setup(o => o.GetMovies("GetPerItemAPI", out httpStatusCode)).Returns(movie);
            webJetWrapperServiceMock.Setup(o => o.GetMovies("GetPerItemAPI", out httpStatusCode)).Returns(movie2);
            movieServicesFactoryMock.Setup(o => o.GetConfigurationService()).Returns(configurationServiceMock.Object);
            movieServicesFactoryMock.Setup(o => o.GetWebJetWrapperService()).Returns(webJetWrapperServiceMock.Object);

            MoviesController movieController = new MoviesController(movieServicesFactoryMock.Object);

            movieController.Request = new HttpRequestMessage();
            movieController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            HttpResponseMessage responseMessage = movieController.Get("cw0076759", "cinemaworld");
            Assert.AreEqual(responseMessage.StatusCode, HttpStatusCode.InternalServerError);
        }

        [TestMethod]
        public void TestGetPerMovie_ReturnNotFound()
        {
            string movieListString = "[{\"Title\": \"Star Wars: Episode IV - A New Hope\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}]";
            string movieListString2 = "[{\"Title\": \"Star Wars: Episode IV - A New Hope 2\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}]";

            string movie = "{\"Title\": \"Star Wars: Episode IV - A New Hope\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}";
            string movie2 = "{\"Title\": \"Star Wars: Episode IV - A New Hope2\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}";

            Mock<IMovieServicesFactory> movieServicesFactoryMock = new Mock<IMovieServicesFactory>();
            Mock<IConfigurationService> configurationServiceMock = new Mock<IConfigurationService>();

            IDictionary<string, IProvider> providersDictionary = new Dictionary<string, IProvider>();         
            configurationServiceMock.Setup(o => o.ProvidersDictionary).Returns(providersDictionary);

            Mock<IWebJetWrapperService> webJetWrapperServiceMock = new Mock<IWebJetWrapperService>();
            HttpStatusCode httpStatusCode = HttpStatusCode.OK;
            webJetWrapperServiceMock.Setup(o => o.GetMovies("GetPerItemAPI", out httpStatusCode)).Returns(movie);
            webJetWrapperServiceMock.Setup(o => o.GetMovies("GetPerItemAPI", out httpStatusCode)).Returns(movie2);
            movieServicesFactoryMock.Setup(o => o.GetConfigurationService()).Returns(configurationServiceMock.Object);
            movieServicesFactoryMock.Setup(o => o.GetWebJetWrapperService()).Returns(webJetWrapperServiceMock.Object);

            MoviesController movieController = new MoviesController(movieServicesFactoryMock.Object);

            movieController.Request = new HttpRequestMessage();
            movieController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            HttpResponseMessage responseMessage = movieController.Get("cw0076759", "cinemaworld");
            Assert.AreEqual(responseMessage.StatusCode, HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void TestGetPerMovie_InternalServerErrorWhenException()
        {
            string movieListString = "[{\"Title\": \"Star Wars: Episode IV - A New Hope\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}]";
            string movieListString2 = "[{\"Title\": \"Star Wars: Episode IV - A New Hope 2\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}]";

            string movie = "{\"Title\": \"Star Wars: Episode IV - A New Hope\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}";
            string movie2 = "{\"Title\": \"Star Wars: Episode IV - A New Hope2\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}";

            Mock<IMovieServicesFactory> movieServicesFactoryMock = new Mock<IMovieServicesFactory>();
            Mock<IConfigurationService> configurationServiceMock = new Mock<IConfigurationService>();

            IDictionary<string, IProvider> providersDictionary = new Dictionary<string, IProvider>();
            configurationServiceMock.Setup(o => o.ProvidersDictionary).Returns(providersDictionary);
            providersDictionary.Add("cinemaworld", new Provider() { Name = "cinemaworld", GetAPI = "GetAPI", GetPerItemAPI = "GetPerItemAPI" });
            providersDictionary.Add("filmworld", new Provider() { Name = "filmworld", GetAPI = "GetAPI2", GetPerItemAPI = "GetPerItemAPI2" });

            Mock<IWebJetWrapperService> webJetWrapperServiceMock = new Mock<IWebJetWrapperService>();
            HttpStatusCode httpStatusCode = HttpStatusCode.OK;
            webJetWrapperServiceMock.Setup(o => o.GetMovies("GetPerItemAPI", out httpStatusCode)).Throws(new InvalidOperationException());
            webJetWrapperServiceMock.Setup(o => o.GetMovies("GetPerItemAPI", out httpStatusCode)).Throws(new InvalidOperationException());
            movieServicesFactoryMock.Setup(o => o.GetConfigurationService()).Returns(configurationServiceMock.Object);
            movieServicesFactoryMock.Setup(o => o.GetWebJetWrapperService()).Returns(webJetWrapperServiceMock.Object);

            MoviesController movieController = new MoviesController(movieServicesFactoryMock.Object);

            movieController.Request = new HttpRequestMessage();
            movieController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            HttpResponseMessage responseMessage = movieController.Get("cw0076759", "cinemaworld");
            Assert.AreEqual(responseMessage.StatusCode, HttpStatusCode.InternalServerError);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGetPerMovie_NullID()
        {
            string movieListString = "[{\"Title\": \"Star Wars: Episode IV - A New Hope\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}]";
            string movieListString2 = "[{\"Title\": \"Star Wars: Episode IV - A New Hope 2\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}]";

            string movie = "{\"Title\": \"Star Wars: Episode IV - A New Hope\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}";
            string movie2 = "{\"Title\": \"Star Wars: Episode IV - A New Hope2\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}";

            Mock<IMovieServicesFactory> movieServicesFactoryMock = new Mock<IMovieServicesFactory>();
            Mock<IConfigurationService> configurationServiceMock = new Mock<IConfigurationService>();

            IDictionary<string, IProvider> providersDictionary = new Dictionary<string, IProvider>();
            configurationServiceMock.Setup(o => o.ProvidersDictionary).Returns(providersDictionary);
            providersDictionary.Add("cinemaworld", new Provider() { Name = "cinemaworld", GetAPI = "GetAPI", GetPerItemAPI = "GetPerItemAPI" });
            providersDictionary.Add("filmworld", new Provider() { Name = "filmworld", GetAPI = "GetAPI2", GetPerItemAPI = "GetPerItemAPI2" });

            Mock<IWebJetWrapperService> webJetWrapperServiceMock = new Mock<IWebJetWrapperService>();
            HttpStatusCode httpStatusCode = HttpStatusCode.OK;
            webJetWrapperServiceMock.Setup(o => o.GetMovies("GetPerItemAPI", out httpStatusCode)).Throws(new InvalidOperationException());
            webJetWrapperServiceMock.Setup(o => o.GetMovies("GetPerItemAPI", out httpStatusCode)).Throws(new InvalidOperationException());
            movieServicesFactoryMock.Setup(o => o.GetConfigurationService()).Returns(configurationServiceMock.Object);
            movieServicesFactoryMock.Setup(o => o.GetWebJetWrapperService()).Returns(webJetWrapperServiceMock.Object);

            MoviesController movieController = new MoviesController(movieServicesFactoryMock.Object);

            movieController.Request = new HttpRequestMessage();
            movieController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            HttpResponseMessage responseMessage = movieController.Get(null, "cinemaworld");
            Assert.AreEqual(responseMessage.StatusCode, HttpStatusCode.InternalServerError);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGetPerMovie_NullProvider()
        {
            string movieListString = "[{\"Title\": \"Star Wars: Episode IV - A New Hope\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}]";
            string movieListString2 = "[{\"Title\": \"Star Wars: Episode IV - A New Hope 2\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}]";

            string movie = "{\"Title\": \"Star Wars: Episode IV - A New Hope\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}";
            string movie2 = "{\"Title\": \"Star Wars: Episode IV - A New Hope2\",\"Year\": \"1977\",\"ID\": \"cw0076759\",\"Type\": \"movie\",\"Poster\": \"http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg\"}";

            Mock<IMovieServicesFactory> movieServicesFactoryMock = new Mock<IMovieServicesFactory>();
            Mock<IConfigurationService> configurationServiceMock = new Mock<IConfigurationService>();

            IDictionary<string, IProvider> providersDictionary = new Dictionary<string, IProvider>();
            configurationServiceMock.Setup(o => o.ProvidersDictionary).Returns(providersDictionary);
            providersDictionary.Add("cinemaworld", new Provider() { Name = "cinemaworld", GetAPI = "GetAPI", GetPerItemAPI = "GetPerItemAPI" });
            providersDictionary.Add("filmworld", new Provider() { Name = "filmworld", GetAPI = "GetAPI2", GetPerItemAPI = "GetPerItemAPI2" });

            Mock<IWebJetWrapperService> webJetWrapperServiceMock = new Mock<IWebJetWrapperService>();
            HttpStatusCode httpStatusCode = HttpStatusCode.OK;
            webJetWrapperServiceMock.Setup(o => o.GetMovies("GetPerItemAPI", out httpStatusCode)).Throws(new InvalidOperationException());
            webJetWrapperServiceMock.Setup(o => o.GetMovies("GetPerItemAPI", out httpStatusCode)).Throws(new InvalidOperationException());
            movieServicesFactoryMock.Setup(o => o.GetConfigurationService()).Returns(configurationServiceMock.Object);
            movieServicesFactoryMock.Setup(o => o.GetWebJetWrapperService()).Returns(webJetWrapperServiceMock.Object);

            MoviesController movieController = new MoviesController(movieServicesFactoryMock.Object);

            movieController.Request = new HttpRequestMessage();
            movieController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            HttpResponseMessage responseMessage = movieController.Get("cw0076759", null);
            Assert.AreEqual(responseMessage.StatusCode, HttpStatusCode.InternalServerError);
        }


    }
}
