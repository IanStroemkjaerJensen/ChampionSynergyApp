using ChampionSynergy.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharpClient.Controllers;
using RestSharpClient.Models;
using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Collections;

namespace ChampionSynergy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private SummonerController _summonerClient = new SummonerController();
        private MatchController _matchClient = new MatchController();
        
        private int? teamId;


        List<Teammate> teammates = new List<Teammate>();
        List<Enemy> enemies = new List<Enemy>();

        List<Teammate> teammatesSortedByWins = new List<Teammate>();
        List<Enemy> enemiesSortedByWins = new List<Enemy>();
        List<Teammate> teammatesSortedByLosses = new List<Teammate>();
        List<Enemy> enemiesSortedByLosses = new List<Enemy>();

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
                        teammate.Role = match.Info.Participants[i].TeamPosition;
                        teammate.Wins = 0;
                        teammate.Losses = 0;
                        teammate.Played = 0;

                        if (teammate.Win == true)
                        {
                            Teammate existingTeammate = teammates.FirstOrDefault(t => t.ChampionName == teammate.ChampionName);
                            if (existingTeammate != null)
                            {
                                existingTeammate.Wins++;
                                existingTeammate.Played++;
                                }
                            else
                            {
                                teammates.Add(teammate);
                            }

                        }
                      
                        if (teammate.Win != true)
                        {
                                     
                                Teammate existingTeammate = teammates.FirstOrDefault(t => t.ChampionName == teammate.ChampionName);
                            if (existingTeammate != null)
                            {
                                existingTeammate.Losses++;
                                existingTeammate.Played++;
                                }
                            else
                            {
                                teammates.Add(teammate);
                            }
                        }                             
                    }

                    if (match.Info.Participants[i].TeamId != teamId)
                    {
                        Enemy enemy = new Enemy();
                        enemy.TeamId = match.Info.Participants[i].TeamId;
                        enemy.SummonerName = match.Info.Participants[i].SummonerName;
                        enemy.ChampionName = match.Info.Participants[i].ChampionName;
                        enemy.Win = match.Info.Participants[i].Win;
                        enemy.Role = match.Info.Participants[i].TeamPosition;
                        enemy.Wins = 0;
                        enemy.Losses = 0;
                        enemy.Played = 0;   


                        if (enemy.Win == true)
                        {
                            Enemy existingEnemy = enemies.FirstOrDefault(e => e.ChampionName == enemy.ChampionName);
                            if (existingEnemy != null)
                            {
                                existingEnemy.Wins++;
                                existingEnemy.Played++;
                            }
                            else
                            {
                                enemies.Add(enemy);
                            }
                        }

                        if (enemy.Win != true)
                        {
                            Enemy existingEnemy = enemies.FirstOrDefault(e => e.ChampionName == enemy.ChampionName);
                            if (existingEnemy != null)
                            {
                                existingEnemy.Losses++;
                                existingEnemy.Played++; 
                            }
                            else
                            {
                                enemies.Add(enemy);
                            }
                        }

                    }

                }

            }

            teammatesSortedByWins = teammates.OrderByDescending(t => t.Wins).ToList();
            enemiesSortedByLosses = enemies.OrderByDescending(t => t.Losses).ToList();
            enemiesSortedByWins = enemies.OrderByDescending(t => t.Wins).ToList();
            teammatesSortedByLosses = teammates.OrderByDescending(t => t.Losses).ToList();

            var topSevenWinsByTeammates = teammatesSortedByWins.Take(7);
            var topSevenLossesByEnemies = enemiesSortedByLosses.Take(7);
            var topSevenWinsByEnemies = enemiesSortedByWins.Take(7);
            var topSevenLossesByTeammates = teammatesSortedByLosses.Take(7);
            
            ViewBag.winsByTeammates = topSevenWinsByTeammates;
            ViewBag.lossesByEnemies = topSevenLossesByEnemies;
            ViewBag.winsByEnemies = topSevenWinsByEnemies;
            ViewBag.lossesByTeammates = topSevenLossesByTeammates;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}