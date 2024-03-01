using RestSharp;
using RestSharpClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Info = RestSharpClient.Models.Info;
using Match = RestSharpClient.Models.Match;

namespace RestSharpClient.Controllers
{
    public class MatchController
    {
        private RestClient _client;
        private readonly string _apiKeyFromConfig = File.ReadAllText(@"C:\\Users\\Ian\\source\\repos\\ChampionSynergyApp\\RestSharpClient\\Config\\ApiKeyExample.txt");
        

        public MatchController()
        {
            _client = new RestClient("https://europe.api.riotgames.com");
        }

        public List<string> SearchForMatchList(Account account)
        {
            //sends a GET request to ""

            RestRequest request = new RestRequest($"/lol/match/v5/matches/by-puuid/{account.Puuid}/ids");

            request.AddQueryParameter("api_key", _apiKeyFromConfig);
            request.AddQueryParameter("count", 95);

            var response = _client.Get(request);

            string jsonString = response.Content;

            List<string> _matchList = JsonSerializer.Deserialize<List<string>>(jsonString);

            return _matchList;
        }

        public Match SearchMatch(String matches)
        {

            //sends a GET request to ""

            RestRequest request = new RestRequest($"/lol/match/v5/matches/{matches}");
            request.AddQueryParameter("api_key", _apiKeyFromConfig);

            var response = _client.Get(request);

            string jsonString = response.Content;

            Match match = JsonSerializer.Deserialize<Match>(jsonString);

            return match;
        }

    }
}
