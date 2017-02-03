using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoviesAPI.Models
{
    public class MovieSet: IMovieSet
    {
        public string Movies { get; set; }
        public string Provider { get; set; }
    }
}