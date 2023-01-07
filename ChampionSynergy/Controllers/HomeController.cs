using ChampionSynergy.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharpClient.Controllers;
using RestSharpClient.Models;
using System.Diagnostics;

namespace ChampionSynergy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private SummonerController _client = new SummonerController();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Startpage()
        {
            return View();
        }

        public IActionResult SearchSummonerName(String summonerName)
        {
            SummonerModel summonerWithPuuid = _client.SearchForPuuid(summonerName);
            

            //SummonerModel? summonerModel =
              //  JsonSerializer.Deserialize<SummonerModel>(jsonString);

            return View();
        }











        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}