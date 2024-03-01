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
        private readonly string _apiKeyFromConfig = File.ReadAllText(@"C:\\Users\\Ian\\source\\repos\\ChampionSynergyApp\\RestSharpClient\\Config\\ApiKeyExample.txt");
        

        public SummonerController()
        {
            _client = new RestClient("https://europe.api.riotgames.com"); 
        }

        public Account SearchForPuuid(string summonerName, string tagLine)

        //
        {
            //sends a GET request to "" 

            //Deprecated
            //RestRequest request = new RestRequest($"/lol/summoner/v4/summoners/by-name/{summonerName}");
            RestRequest request = new RestRequest($"/riot/account/v1/accounts/by-riot-id/{summonerName}/{tagLine}");
            request.AddQueryParameter("api_key", _apiKeyFromConfig);
            
            var response = _client.Get(request);

            string jsonString = response.Content;

            Account accountModel = JsonSerializer.Deserialize<Account>(jsonString);

            return accountModel;
        }

    }

}
