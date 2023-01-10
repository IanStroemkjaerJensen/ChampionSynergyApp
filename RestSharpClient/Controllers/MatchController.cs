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
        private readonly String _apiKey = "RGAPI-9721845c-9b83-4b1e-a992-815b105e690b";
        
          


        public MatchController()
        {
            _client = new RestClient("https://europe.api.riotgames.com");
        }

        public List<string> SearchForMatchList(Summoner summoner)
        {
            //sends a GET request to ""

            RestRequest request = new RestRequest($"/lol/match/v5/matches/by-puuid/{summoner.Puuid}/ids");
            request.AddQueryParameter("api_key", _apiKey);
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
            request.AddQueryParameter("api_key", _apiKey);

            var response = _client.Get(request);

            string jsonString = response.Content;

            Match match = JsonSerializer.Deserialize<Match>(jsonString);

            return match;
        }

    }
}
