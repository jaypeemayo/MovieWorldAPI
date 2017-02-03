using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Models
{
    public interface IMovieSet
    {
        string Movies { get; set; }
        string Provider { get; set; }
    }
}
