using ChampionSynergy.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharpClient.Controllers;
using RestSharpClient.Models;
using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Collections;
using Microsoft.AspNetCore.Http;

namespace ChampionSynergy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private SummonerController _summonerClient = new SummonerController();
        private MatchController _matchClient = new MatchController();
        
        private int? teamId;

        private Match match;

        List<Teammate> teammates = new List<Teammate>();
        List<Enemy> enemies = new List<Enemy>();

        List<Teammate> junglerTeammates = new List<Teammate>();
        List<Enemy> junglerEnemies = new List<Enemy>();

        //General
        List<Teammate> teammatesSortedByWins = new List<Teammate>();
        List<Enemy> enemiesSortedByWins = new List<Enemy>();
        List<Teammate> teammatesSortedByLosses = new List<Teammate>();
        List<Enemy> enemiesSortedByLosses = new List<Enemy>();

        //Junglers
        List<Teammate> junglerTeammatesSortedByWins = new List<Teammate>();
        List<Enemy> junglerEnemiesSortedByWins = new List<Enemy>();
        List<Teammate> junglerTeammatesSortedByLosses = new List<Teammate>();
        List<Enemy> junglerEnemiesSortedByLosses = new List<Enemy>();


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

        public IActionResult SearchSummoner(Account accountModel)
        {
            Account account = _summonerClient.SearchForPuuid(accountModel.Name, accountModel.Tagline);

            _matchClient.SearchForMatchList(account);
            
            List<string> matchList = _matchClient.SearchForMatchList(account);

            foreach (string matchInList in matchList)
            {
                Match match = new Match();
                match = _matchClient.SearchMatch(matchInList);

                //HttpContext.Session.SetObject("MatchData", match);


                for (int i = 0; i < match.Info.Participants.Count; i++)
                {
                    if (match.Info.Participants[i].SummonerName == accountModel.Name)
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
        /*
         * To be continued
         * Needs to find a way to pass variable from 1 actionresult method to another without redirection and in different requests
         * 
        public IActionResult Jungle()
        {


            Match matchFromSession = HttpContext.Session.GetObject<Match>("MatcData");

            for (int i = 0; i < matchFromSession.Info.Participants.Count; i++)
                {


                     if(matchFromSession.Info.Participants[i].TeamId == teamId)
                     {

                        Teammate teammate = new Teammate();
                        teammate.TeamId = matchFromSession.Info.Participants[i].TeamId;
                        teammate.SummonerName = matchFromSession.Info.Participants[i].SummonerName;
                        teammate.ChampionName = matchFromSession.Info.Participants[i].ChampionName;
                        teammate.Win = matchFromSession.Info.Participants[i].Win;
                        teammate.Role = matchFromSession.Info.Participants[i].TeamPosition;
                        teammate.Wins = 0;
                        teammate.Losses = 0;
                        teammate.Played = 0;

                        if (teammate.Role =="Jungle" && teammate.Win == true)
                        {
                            Teammate existingTeammate = teammates.FirstOrDefault(t => t.ChampionName == teammate.ChampionName);
                            if (existingTeammate != null)
                            {
                                existingTeammate.Wins++;
                                existingTeammate.Played++;
                            }
                            else
                            {
                                junglerTeammates.Add(teammate);
                            }

                        }

                        if (teammate.Role == "Jungle" && teammate.Win != true)
                        {

                            Teammate existingTeammate = teammates.FirstOrDefault(t => t.ChampionName == teammate.ChampionName);
                            if (existingTeammate != null)
                            {
                                existingTeammate.Losses++;
                                existingTeammate.Played++;
                            }
                            else
                            {
                                junglerTeammates.Add(teammate);
                            }
                        }
                    }

                    if (matchFromSession.Info.Participants[i].TeamId != teamId)
                    {
                        Enemy enemy = new Enemy();
                        enemy.TeamId = matchFromSession.Info.Participants[i].TeamId;
                        enemy.SummonerName = matchFromSession.Info.Participants[i].SummonerName;
                        enemy.ChampionName = matchFromSession.Info.Participants[i].ChampionName;
                        enemy.Win = matchFromSession.Info.Participants[i].Win;
                        enemy.Role = matchFromSession.Info.Participants[i].TeamPosition;
                        enemy.Wins = 0;
                        enemy.Losses = 0;
                        enemy.Played = 0;


                        if (enemy.Role == "Jungle" && enemy.Win == true)
                        {
                            Enemy existingEnemy = enemies.FirstOrDefault(e => e.ChampionName == enemy.ChampionName);
                            if (existingEnemy != null)
                            {
                                existingEnemy.Wins++;
                                existingEnemy.Played++;
                            }
                            else
                            {
                                junglerEnemies.Add(enemy);
                            }
                        }

                        if (enemy.Role == "Jungle" && enemy.Win != true)
                        {
                            Enemy existingEnemy = enemies.FirstOrDefault(e => e.ChampionName == enemy.ChampionName);
                            if (existingEnemy != null)
                            {
                                existingEnemy.Losses++;
                                existingEnemy.Played++;
                            }
                            else
                            {
                                junglerEnemies.Add(enemy);
                            }
                        }

                    }

                }

            junglerTeammatesSortedByWins = junglerTeammates.OrderByDescending(t => t.Wins).ToList();
            junglerEnemiesSortedByLosses = junglerEnemies.OrderByDescending(t => t.Losses).ToList();
            junglerEnemiesSortedByWins = junglerEnemies.OrderByDescending(t => t.Wins).ToList();
            junglerTeammatesSortedByLosses = junglerTeammates.OrderByDescending(t => t.Losses).ToList();

            var topSevenWinsByJunglerTeammates = junglerTeammatesSortedByWins.Take(7);
            var topSevenLossesByJunglerEnemies = junglerEnemiesSortedByLosses.Take(7);
            var topSevenWinsByJunglerEnemies = junglerEnemiesSortedByWins.Take(7);
            var topSevenLossesByJunglerTeammates = junglerTeammatesSortedByLosses.Take(7);

            ViewBag.winsByJunglerTeammates = topSevenWinsByJunglerTeammates;
            ViewBag.lossesByJunglerEnemies = topSevenLossesByJunglerEnemies;
            ViewBag.winsByJunglerEnemies = topSevenWinsByJunglerEnemies;
            ViewBag.lossesByJunglerTeammates = topSevenLossesByJunglerTeammates;

            return View();
        }
*/
            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}