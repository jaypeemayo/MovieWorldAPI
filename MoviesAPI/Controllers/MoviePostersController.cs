using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace MoviesAPI.Controllers
{
    public class MoviePostersController : ApiController
    {
        private static Dictionary<string, byte[]> imageCache = new Dictionary<string, byte[]>();
        // GET: api/MoviePosters/5
        public HttpResponseMessage Get(string id)
        {
            try
            {
                byte[] bytes;
                if (!imageCache.TryGetValue(id, out bytes))
                {
                    WebClient wc = new WebClient();
                    bytes = wc.DownloadData(id);
                }

                imageCache[id] = bytes;
                MemoryStream ms = new MemoryStream(bytes);
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(ms.ToArray());
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
             
                return result;

            }
            catch (WebException e)
            {
                if (((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.NotFound)
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }

                throw;
            }
        }
    }
}
