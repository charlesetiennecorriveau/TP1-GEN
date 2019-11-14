using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TP1_GEN.Models
{
    public class APICaller
    {
        public static async Task<List<FilmOverview>> GetNowPlayingAsync(int page)
        {
            // This got our key in it OwO
            string apiUrl = "https://api.themoviedb.org/3/movie/now_playing?api_key=42ea10e4b84b36c36a064a410b108b2f&page=1";

            List<FilmOverview> filmsNowPlaying = new List<FilmOverview>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                var data = await response.Content.ReadAsStringAsync();
                JObject jObject = JObject.Parse(data);

                

                foreach (var film in jObject["results"])
                {
                    filmsNowPlaying.Add(new FilmOverview
                    {
                        PosterUrl = film["poster_path"].ToString(),
                        Title = film["title"].ToString()
                    });
                }


            }

            return filmsNowPlaying;
        }
    }
}
