using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP1_GEN.Models
{
    public class Film
    {
        public string PosterUrl { get; }
        public string Title { get; }
        public string Summary { get; }
        public List<string> Casting { get; }
        public List<string> Team { get; }
        public float Rating { get; }
    }
}
