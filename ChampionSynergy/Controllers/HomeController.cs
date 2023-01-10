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
        
        private int? teamId;

        Dictionary<string, int> winsByTeammate = new Dictionary<string, int>();
        Dictionary<string, int> lossesByTeammate = new Dictionary<string, int>();

        Dictionary<string, int> winsByEnemy = new Dictionary<string, int>();
        Dictionary<string, int> lossesByEnemy = new Dictionary<string, int>();


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

        public IActionResult SearchSummoner(Summoner summonerModel)
        {
            Summoner summoner = _summonerClient.SearchForPuuid(summonerModel.Name);

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
                        teamId = match.Info.Participants[i].TeamId;

                        match.Info.Participants.RemoveAt(i);
                        continue;
                    }

                    if (match.Info.Participants[i].TeamId == teamId)
                    {
                        Teammate teammate = new Teammate();
                        teammate.TeamId = match.Info.Participants[i].TeamId;
                        teammate.SummonerName = match.Info.Participants[i].SummonerName;
                        teammate.ChampionName = match.Info.Participants[i].ChampionName;
                        teammate.Win = match.Info.Participants[i].Win;

                        if (teammate.Win == true)
                        {
                            if (!winsByTeammate.ContainsKey(teammate.ChampionName))
                            {
                                winsByTeammate[teammate.ChampionName] = 0;
                            }
                            winsByTeammate[teammate.ChampionName]++;
                        }

                        if (teammate.Win != true)
                        {
                            if (!lossesByTeammate.ContainsKey(teammate.ChampionName))
                            {
                                lossesByTeammate[teammate.ChampionName] = 0;
                            }
                            lossesByTeammate[teammate.ChampionName]++;
                        }
                    }

                    if (match.Info.Participants[i].TeamId != teamId)
                    {
                        Enemy enemy = new Enemy();
                        enemy.TeamId = match.Info.Participants[i].TeamId;
                        enemy.SummonerName = match.Info.Participants[i].SummonerName;
                        enemy.ChampionName = match.Info.Participants[i].ChampionName;
                        enemy.Win = match.Info.Participants[i].Win;

                        if (enemy.Win == true)
                        {
                            if (!winsByEnemy.ContainsKey(enemy.ChampionName))
                            {
                                winsByEnemy[enemy.ChampionName] = 0;
                            }
                            winsByEnemy[enemy.ChampionName]++;
                        }

                        if (enemy.Win != true)
                        {
                            if (!lossesByEnemy.ContainsKey(enemy.ChampionName))
                            {
                                lossesByEnemy[enemy.ChampionName] = 0;
                            }
                            lossesByEnemy[enemy.ChampionName]++;
                        }
                    }

                }

            }

            winsByTeammate = winsByTeammate.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            lossesByTeammate = lossesByTeammate.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            winsByEnemy = winsByEnemy.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            lossesByEnemy = lossesByEnemy.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            var topSevenWinsByTeammate = winsByTeammate.Take(10);
            var topSevenLossesByTeammate = lossesByTeammate.Take(10);
            var topSevenWinsByEnemy = winsByEnemy.Take(10);
            var topSevenLossesByEnemy = lossesByEnemy.Take(10);


            ViewBag.winsByTeammate = topSevenWinsByTeammate;
            ViewBag.lossesByTeammate = topSevenLossesByTeammate;
            ViewBag.winsByEnemy = topSevenWinsByEnemy;
            ViewBag.lossesByEnemy = topSevenLossesByEnemy;


            return View();
        }











        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}