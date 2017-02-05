using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Services
{
    public interface IHttpService
    {
        string Get(string url, out HttpStatusCode statusCode);
    }
}
