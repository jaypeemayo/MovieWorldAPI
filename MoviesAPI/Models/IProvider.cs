using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Models
{
    public interface IProvider
    {
        string Name { get; set; }
        string GetAPI { get; set; }
        string GetPerItemAPI { get; set; }
    }
}
