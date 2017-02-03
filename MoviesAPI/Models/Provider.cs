using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoviesAPI.Models
{
    public class Provider: IProvider
    {
        public string Name { get; set; }
        public string GetAPI { get; set; }
        public string GetPerItemAPI { get; set; }
    }
}