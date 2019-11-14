using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP1_GEN.Models
{
    public class Film
    {
        public string PosterUrl { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public List<string> Casting { get; set; }
        public List<string> Team { get; set; }
        public float Rating { get; set; }
    }
}
