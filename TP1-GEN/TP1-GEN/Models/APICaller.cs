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
        const string ApiKey = "42ea10e4b84b36c36a064a410b108b2f";

        public static async Task<List<FilmOverview>> GetNowPlayingAsync(int page)
        {
            // This got our key in it OwO
            string apiUrl = $"https://api.themoviedb.org/3/movie/now_playing?api_key={ApiKey}&page={page}";

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
                        Title = film["title"].ToString(),
                        Id = (int)film["id"]
                    }); ;
                }
            }
            return filmsNowPlaying;
        }

        public static async Task<Film> getFilmDetailsAsync(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = $"https://api.themoviedb.org/3/movie/{id}?api_key={ApiKey}";

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                client.BaseAddress = new Uri(apiUrl);
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                var data = await response.Content.ReadAsStringAsync();
                JObject filmResult = JObject.Parse(data);


                //apiUrl = $"https://api.themoviedb.org/3/movie/{id}?api_key={ApiKey}";

                //client.BaseAddress = new Uri(apiUrl);
                //response = await client.GetAsync(apiUrl);
                //data = await response.Content.ReadAsStringAsync();

                Film film = new Film
                {
                    PosterUrl = (string)filmResult["poster_path"],
                    Rating = (int)filmResult["vote_average"],
                    Summary = (string)filmResult["overview"],
                    Title = (string)filmResult["title"]
                };

                return film;
            }
        }
    }
}
