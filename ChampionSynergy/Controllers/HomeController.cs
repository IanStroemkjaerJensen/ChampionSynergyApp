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

        private SummonerController _summonerClient = new SummonerController();
        private MatchController _matchClient = new MatchController();

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

        public IActionResult SearchSummoner(SummonerModel summonerModel)
        {
            SummonerModel summoner = _summonerClient.SearchForPuuid(summonerModel.name);

            _matchClient.SearchForMatchList(summoner);
            
            List<string> matchList = _matchClient.SearchForMatchList(summoner);

            foreach (string matchInList in matchList)
            {
                
                Info info = _matchClient.SearchMatch(matchInList);

               
            }
           

            return View();
        }











        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}