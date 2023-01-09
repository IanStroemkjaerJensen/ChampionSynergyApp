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

namespace RestSharpClient.Controllers
{
    public class MatchController
    {
        private RestClient _client;
        private readonly String _apiKey = "RGAPI-9bc8d206-8af5-4c27-af81-48931e5c3071";
        private List<string> _matchList;
        private Info _info;   


        public MatchController()
        {
            _client = new RestClient("https://europe.api.riotgames.com");
        }

        public List<string> SearchForMatchList(SummonerModel summoner)
        {
            //sends a GET request to ""

            RestRequest request = new RestRequest($"/lol/match/v5/matches/by-puuid/{summoner.puuid}/ids");
            request.AddQueryParameter("api_key", _apiKey);

            var response = _client.Get(request);

            string jsonString = response.Content;   

            _matchList = JsonSerializer.Deserialize<List<string>>(jsonString);

            return _matchList;
        }

        public Info SearchMatch(String matches)
        {

            //sends a GET request to ""

            RestRequest request = new RestRequest($"/lol/match/v5/matches/{matches}");
            request.AddQueryParameter("api_key", _apiKey);

            var response = _client.Get(request);

            string jsonString = response.Content;

            _info = JsonSerializer.Deserialize<Info>(jsonString);

            return _info;
        }

    }
}
