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
        private readonly String _apiKey = "RGAPI-ef5d7fad-81d0-44d0-896b-1386fc660b40";
        

        public SummonerController()
        {
            _client = new RestClient("https://euw1.api.riotgames.com");
        }

        public SummonerModel SearchForPuuid(String summonerName)
        {
            //sends a GET request to ""
            RestRequest request = new RestRequest($"/lol/summoner/v4/summoners/by-name/agurin");
            request.AddQueryParameter("api_key", _apiKey);
            
            var response = _client.Get(request);

            string jsonString = response.Content;

            SummonerModel summonerModel = JsonSerializer.Deserialize<SummonerModel>(jsonString);

            return summonerModel;
        }

    }

}
