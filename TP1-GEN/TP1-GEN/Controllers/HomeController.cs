using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using TP1_GEN.Infrastructure;
using TP1_GEN.Models;

namespace TP1_GEN.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index(int page, string search)
        {
            ViewData["Page"] = page;
            ViewData["Search"] = search;
            if (search == null)
            {
                List<FilmOverview> listFilms = await APICaller.GetNowPlayingAsync(page);
                return View(listFilms);
            }
            else
            {
                List<FilmOverview> listFilms = await APICaller.SearchAsync(search);
                return View(listFilms);
            }
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public async Task<ActionResult> Details(int Id)
        {
            Film details = await APICaller.GetFilmDetailsAsync(Id);
            return View(details);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Trailer(string title)
        {
            string url = await APICaller.GetTrailerAsync(title);
            return Redirect(url);
        }
    }
}
