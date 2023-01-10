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
        
        private int? teammatesId;

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
            SummonerModel summoner = _summonerClient.SearchForPuuid(summonerModel.Name);

            _matchClient.SearchForMatchList(summoner);
            
            List<string> matchList = _matchClient.SearchForMatchList(summoner);

            foreach (string matchInList in matchList)
            {
                Match match = new Match();
                match = _matchClient.SearchMatch(matchInList);

                for (int i = 0; i < match.Info.Participants.Count; i++)
                {
                    if (match.Info.Participants[i].SummonerName == summonerModel.Name)
                    {
                        int? teammatesId = match.Info.Participants[i].TeamId;

                        match.Info.Participants.RemoveAt(i);
                        continue;
                    }

                    if (match.Info.Participants[i].TeamId == teammatesId)
                    {
                        Teammate teammate = new Teammate();
                        teammate.TeamId = match.Info.Participants[i].TeamId;
                        teammate.SummonerName = match.Info.Participants[i].SummonerName;
                        teammate.ChampionName = match.Info.Participants[i].ChampionName;
                        teammate.Win = match.Info.Participants[i].Win;

                        List<Teammate> teammates = new List<Teammate>();
                        teammates.Add(teammate);
                    }

                    if (match.Info.Participants[i].TeamId != teammatesId)
                    {
                        Enemy enemy = new Enemy();
                        enemy.TeamId = match.Info.Participants[i].TeamId;
                        enemy.SummonerName = match.Info.Participants[i].SummonerName;
                        enemy.ChampionName = match.Info.Participants[i].ChampionName;
                        enemy.Win = match.Info.Participants[i].Win;

                        List<Enemy> enemies = new List<Enemy>();
                        enemies.Add(enemy);
                    }

                }




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