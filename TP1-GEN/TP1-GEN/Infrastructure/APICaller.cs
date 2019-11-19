using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TP1_GEN.Models;

namespace TP1_GEN.Infrastructure
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

            film.Id = id;
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

        public static async Task<string> GetTrailerAsync(string title)
        {
            using (HttpClient client = new HttpClient())
            {
                string ytApi = "AIzaSyBaPxrQmBq0Iyd5A_aAgDCSit3IaL4t6xc";
                string apiUrl = $"https://www.googleapis.com/youtube/v3/search?part=snippet&maxResults=1&q={title}%20Trailer&topicId=%2Fm%2F02vxn&type=video&key={ytApi}";

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                client.BaseAddress = new Uri(apiUrl);
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                var data = await response.Content.ReadAsStringAsync();
                JObject filmResult = JObject.Parse(data);

                string trailerUrl = "https://www.youtube.com/watch?v=" + (string)filmResult["items"][0]["id"]["videoId"];

                return trailerUrl;
            }
        }

        public static async Task<string> GetEbayListingAsync(string title)
        {
            using (HttpClient client = new HttpClient())
            {
                string ebayApi = "TeamOrth-movieGEN-PRD-2387e50aa-74952026";
                var apiUrl = $"https://svcs.ebay.com/services/search/FindingService/v1?OPERATION-NAME=findItemsAdvanced&SERVICE-VERSION=1.0.0&SECURITY-APPNAME={ebayApi}&RESPONSE-DATA-FORMAT=JSON&REST-PAYLOAD&keywords={title}%20movie&categoryId=11232&itemFilter(0).name=Condition&itemFilter(0).value=New&paginationInput.entriesPerPage=1";

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                client.BaseAddress = new Uri(apiUrl);
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                var data = await response.Content.ReadAsStringAsync();
                JObject filmResult = JObject.Parse(data);

                //string listingUrl = (string)filmResult["findItemsAdvancedResponse"][0]["searchResult"][0]["item"][0]["viewItemURL"][0];
                string listingUrl = (string)filmResult["findItemsAdvancedResponse"][0]["itemSearchURL"][0];

                return listingUrl;
            }
        }
        public static async Task<List<Review>> GetMovieReviews(string title)
        {
            List<Review> reviews = new List<Review>();
            // TO-DO : NYT Reviews API
            using (HttpClient client = new HttpClient())
            {
                string ApiKeyNYT = "JxpqJSjXKKXShYz3IvbeSuGcUJMGXfLG";
                string apiUrl = $"https://api.nytimes.com/svc/movies/v2/reviews/search.json?query={title}&api-key={ApiKeyNYT}";

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));



                client.BaseAddress = new Uri(apiUrl);
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                var data = await response.Content.ReadAsStringAsync();
                JObject filmResult = JObject.Parse(data);

                foreach (var review in filmResult["results"])
                {
                    reviews.Add(new Review()
                    {
                        Headline = review["headline"].ToString(),
                        Summarry = review["summary_short"].ToString(),
                        Url = review["link"]["url"].ToString()
                    });
                }
                return reviews;
            }
        }
    }
}
