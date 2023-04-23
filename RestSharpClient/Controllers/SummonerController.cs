using RestSharp;
using RestSharpClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace RestSharpClient.Controllers
{
 
    public class SummonerController
    {
        private RestClient _client;
        private readonly String _apiKey = "RGAPI-ab3a62d1-5fb1-4bb8-a0c8-39703ecf8a86";
        

        public SummonerController()
        {
            _client = new RestClient("https://euw1.api.riotgames.com");
        }

        public Summoner SearchForPuuid(String summonerName)
        {
            //sends a GET request to ""
            RestRequest request = new RestRequest($"/lol/summoner/v4/summoners/by-name/{summonerName}");
            request.AddQueryParameter("api_key", _apiKey);
            
            var response = _client.Get(request);

            string jsonString = response.Content;

            Summoner summonerModel = JsonSerializer.Deserialize<Summoner>(jsonString);

            return summonerModel;
        }

    }

}
