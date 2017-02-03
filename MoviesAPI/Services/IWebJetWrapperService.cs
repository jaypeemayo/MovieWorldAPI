using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Services
{
    public interface IWebJetWrapperService
    {
        string GetMovies(string url, out HttpStatusCode statusCode);
    }
}
