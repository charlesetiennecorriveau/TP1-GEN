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
        private const string ApiKey = "42ea10e4b84b36c36a064a410b108b2f";

        public static async Task<List<FilmOverview>> SearchAsync(string search)
        {
            // This got our key in it OwO
            string apiUrl = $"https://api.themoviedb.org/3/search/movie?api_key={ApiKey}&query={search}";

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

        public static async Task<Film> GetFilmDetailsAsync(int id)
        {
            Film film = new Film();
            // Get details
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = $"https://api.themoviedb.org/3/movie/{id}?api_key={ApiKey}";

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                client.BaseAddress = new Uri(apiUrl);
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                var data = await response.Content.ReadAsStringAsync();
                JObject filmResult = JObject.Parse(data);

                film.PosterUrl = (string)filmResult["poster_path"];
                film.Rating = (int)filmResult["vote_average"];
                film.Summary = (string)filmResult["overview"];
                film.Title = (string)filmResult["title"];
                film.BackdropUrl = (string)filmResult["backdrop_path"];
            }

            // Get cast
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = $"https://api.themoviedb.org/3/movie/{id}/credits?api_key={ApiKey}";

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                client.BaseAddress = new Uri(apiUrl);
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                var data = await response.Content.ReadAsStringAsync();
                JObject filmCredits = JObject.Parse(data);

                List<string> casting = new List<string>();
                List<string> team = new List<string>();

                foreach (var cast in filmCredits["cast"])
                    casting.Add((string)cast["name"]);

                foreach (var crew in filmCredits["crew"])
                    team.Add((string)crew["name"]);

                film.Casting = casting;
                film.Team = team;
            }
            return film;
        }
    }
}
