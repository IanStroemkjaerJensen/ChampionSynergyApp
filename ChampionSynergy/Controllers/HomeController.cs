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

        //General
        List<Teammate> teammates = new List<Teammate>();
        List<Enemy> enemies = new List<Enemy>();

        //Junglers
        List<Teammate> junglerTeammates = new List<Teammate>();
        List<Enemy> junglerEnemies = new List<Enemy>();

        //Midlaners
        List<Teammate> midlaneTeammates = new List<Teammate>();
        List<Enemy> midlaneEnemies = new List<Enemy>();

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

        //Midlaners
        List<Teammate> midlaneTeammatesSortedByWins = new List<Teammate>();
        List<Enemy> midlaneEnemiesSortedByWins = new List<Enemy>();
        List<Teammate> midlaneTeammatesSortedByLosses = new List<Teammate>();
        List<Enemy> midlaneEnemiesSortedByLosses = new List<Enemy>();



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

            List<string> matchList = _matchClient.SearchForMatchList(account);



            foreach (string matchFromList in matchList)
            {
                Match match = new Match();
                match = _matchClient.SearchMatch(matchFromList);

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
                                teammate.Wins++;
                                teammate.Played++;
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
                                teammate.Losses++;
                                teammate.Played++;
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
                                enemy.Wins++;
                                enemy.Played++;
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
                                enemy.Losses++;
                                enemy.Played++;
                                enemies.Add(enemy);
                            }
                        }

                    }

                }

            }

            teammatesSortedByWins = teammates.OrderByDescending(t => t.Wins).ToList();
            teammatesSortedByLosses = teammates.OrderByDescending(t => t.Losses).ToList();
            enemiesSortedByWins = enemies.OrderByDescending(t => t.Wins).ToList();
            enemiesSortedByLosses = enemies.OrderByDescending(t => t.Losses).ToList();


            var topSevenWinsByTeammates = teammatesSortedByWins.Take(7);
            var topSevenLossesByTeammates = teammatesSortedByLosses.Take(7);
            var topSevenWinsByEnemies = enemiesSortedByWins.Take(7);
            var topSevenLossesByEnemies = enemiesSortedByLosses.Take(7);

            ViewBag.winsByTeammates = topSevenWinsByTeammates;
            ViewBag.lossesByTeammates = topSevenLossesByTeammates;
            ViewBag.winsByEnemies = topSevenWinsByEnemies;
            ViewBag.lossesByEnemies = topSevenLossesByEnemies;

            ViewBag.Name = account.Name;
            ViewBag.Tagline = account.Tagline;

            return View();
        }

        // To be continued
        // Needs to find a way to pass variable from 1 actionresult method to another without redirection and in different requests

        public IActionResult SearchJungleOnly(Account accountModel)
        {
            Account account = _summonerClient.SearchForPuuid(accountModel.Name, accountModel.Tagline);

            List<string> matchList = _matchClient.SearchForMatchList(account);

            foreach (string matchFromList in matchList)
            {
                Match match = new Match();
                match = _matchClient.SearchMatch(matchFromList);

                for (int i = 0; i < match.Info.Participants.Count; i++)
                {

                    if (match.Info.Participants[i].SummonerName == accountModel.Name)
                    {
                        teamId = match.Info.Participants[i].TeamId;
                        continue;
                    }

                    if (match.Info.Participants[i].TeamId == teamId & match.Info.Participants[i].TeamPosition == "JUNGLE")
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
                            Teammate alreadyExistingChampionTeammate = junglerTeammates.FirstOrDefault(t => t.ChampionName == teammate.ChampionName);
                            if (alreadyExistingChampionTeammate != null)
                            {
                                alreadyExistingChampionTeammate.Wins++;
                                alreadyExistingChampionTeammate.Played++;
                            }
                            else
                            {
                                teammate.Wins++;
                                teammate.Played++;
                                junglerTeammates.Add(teammate);
                            }

                        }


                        if (teammate.Win != true)
                        {

                            Teammate alreadyExistingChampionTeammate = junglerTeammates.FirstOrDefault(t => t.ChampionName == teammate.ChampionName);
                            if (alreadyExistingChampionTeammate != null)
                            {
                                alreadyExistingChampionTeammate.Losses++;
                                alreadyExistingChampionTeammate.Played++;
                            }
                            else
                            {
                                teammate.Losses++;
                                teammate.Played++;
                                junglerTeammates.Add(teammate);
                            }
                        }
                    }

                    if (match.Info.Participants[i].TeamId != teamId & match.Info.Participants[i].TeamPosition == "JUNGLE")
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
                            Enemy alreadyExistingChampionEnemy = junglerEnemies.FirstOrDefault(e => e.ChampionName == enemy.ChampionName);
                            if (alreadyExistingChampionEnemy != null)
                            {
                                alreadyExistingChampionEnemy.Wins++;
                                alreadyExistingChampionEnemy.Played++;
                            }
                            else
                            {
                                enemy.Wins++;
                                enemy.Played++;
                                junglerEnemies.Add(enemy);
                            }
                        }

                        if (enemy.Win != true)
                        {
                            Enemy alreadyExistingChampionEnemy = junglerEnemies.FirstOrDefault(e => e.ChampionName == enemy.ChampionName);
                            if (alreadyExistingChampionEnemy != null)
                            {
                                alreadyExistingChampionEnemy.Losses++;
                                alreadyExistingChampionEnemy.Played++;
                            }
                            else
                            {
                                enemy.Losses++;
                                enemy.Played++;
                                junglerEnemies.Add(enemy);
                            }
                        }

                    }

                }
            }

            junglerTeammatesSortedByWins = junglerTeammates.OrderByDescending(t => t.Wins).ToList();
            junglerTeammatesSortedByLosses = junglerTeammates.OrderByDescending(t => t.Losses).ToList();
            junglerEnemiesSortedByWins = junglerEnemies.OrderByDescending(t => t.Wins).ToList();
            junglerEnemiesSortedByLosses = junglerEnemies.OrderByDescending(t => t.Losses).ToList();

            var topSevenWinsByJunglerTeammates = junglerTeammatesSortedByWins.Take(7);
            var topSevenLossesByJunglerTeammates = junglerTeammatesSortedByLosses.Take(7);
            var topSevenWinsByJunglerEnemies = junglerEnemiesSortedByWins.Take(7);
            var topSevenLossesByJunglerEnemies = junglerEnemiesSortedByLosses.Take(7);

            ViewBag.winsByJunglerTeammates = topSevenWinsByJunglerTeammates;
            ViewBag.lossesByJunglerTeammates = topSevenLossesByJunglerTeammates;
            ViewBag.winsByJunglerEnemies = topSevenWinsByJunglerEnemies;
            ViewBag.lossesByJunglerEnemies = topSevenLossesByJunglerEnemies;


            return View();
        }

        public IActionResult Midlane(Account accountModel)
        {
            Account account = _summonerClient.SearchForPuuid(accountModel.Name, accountModel.Tagline);

            List<string> matchList = _matchClient.SearchForMatchList(account);

            foreach (string matchFromList in matchList)
            {
                Match match = new Match();
                match = _matchClient.SearchMatch(matchFromList);

                for (int i = 0; i < match.Info.Participants.Count; i++)
                {

                    if (match.Info.Participants[i].SummonerName == accountModel.Name)
                    {
                        teamId = match.Info.Participants[i].TeamId;
                        continue;
                    }

                    if (match.Info.Participants[i].TeamId == teamId & match.Info.Participants[i].TeamPosition == "MIDDLE")
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
                            Teammate alreadyExistingChampionTeammate = midlaneTeammates.FirstOrDefault(t => t.ChampionName == teammate.ChampionName);
                            if (alreadyExistingChampionTeammate != null)
                            {
                                alreadyExistingChampionTeammate.Wins++;
                                alreadyExistingChampionTeammate.Played++;
                            }
                            else
                            {
                                teammate.Wins++;
                                teammate.Played++;
                                midlaneTeammates.Add(teammate);
                            }

                        }


                        if (teammate.Win != true)
                        {

                            Teammate alreadyExistingChampionTeammate = midlaneTeammates.FirstOrDefault(t => t.ChampionName == teammate.ChampionName);
                            if (alreadyExistingChampionTeammate != null)
                            {
                                alreadyExistingChampionTeammate.Losses++;
                                alreadyExistingChampionTeammate.Played++;
                            }
                            else
                            {
                                teammate.Losses++;
                                teammate.Played++;
                                midlaneTeammates.Add(teammate);
                            }
                        }
                    }

                    if (match.Info.Participants[i].TeamId != teamId & match.Info.Participants[i].TeamPosition == "MIDDLE")
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
                            Enemy alreadyExistingChampionEnemy = midlaneEnemies.FirstOrDefault(e => e.ChampionName == enemy.ChampionName);
                            if (alreadyExistingChampionEnemy != null)
                            {
                                alreadyExistingChampionEnemy.Wins++;
                                alreadyExistingChampionEnemy.Played++;
                            }
                            else
                            {
                                enemy.Wins++;
                                enemy.Played++;
                                midlaneEnemies.Add(enemy);
                            }
                        }

                        if (enemy.Win != true)
                        {
                            Enemy alreadyExistingChampionEnemy = midlaneEnemies.FirstOrDefault(e => e.ChampionName == enemy.ChampionName);
                            if (alreadyExistingChampionEnemy != null)
                            {
                                alreadyExistingChampionEnemy.Losses++;
                                alreadyExistingChampionEnemy.Played++;
                            }
                            else
                            {
                                enemy.Losses++;
                                enemy.Played++;
                                midlaneEnemies.Add(enemy);
                            }
                        }

                    }

                }
            }

            midlaneTeammatesSortedByWins = midlaneTeammates.OrderByDescending(t => t.Wins).ToList();
            midlaneTeammatesSortedByLosses = midlaneTeammates.OrderByDescending(t => t.Losses).ToList();
            midlaneEnemiesSortedByWins = midlaneEnemies.OrderByDescending(t => t.Wins).ToList();
            midlaneEnemiesSortedByLosses = midlaneEnemies.OrderByDescending(t => t.Losses).ToList();

            var topSevenWinsByMidlaneTeammates = midlaneTeammatesSortedByWins.Take(7);
            var topSevenLossesByMidlaneTeammates = midlaneTeammatesSortedByLosses.Take(7);
            var topSevenWinsByMidlaneEnemies = midlaneEnemiesSortedByWins.Take(7);
            var topSevenLossesByMidlaneEnemies = midlaneEnemiesSortedByLosses.Take(7);

            ViewBag.winsByMidlaneTeammates = topSevenWinsByMidlaneTeammates;
            ViewBag.lossesByMidlaneTeammates = topSevenLossesByMidlaneTeammates;
            ViewBag.winsByMidlaneEnemies = topSevenWinsByMidlaneEnemies;
            ViewBag.lossesByMidlaneEnemies = topSevenLossesByMidlaneEnemies;


            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}