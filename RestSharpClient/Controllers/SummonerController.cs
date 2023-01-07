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

        public SummonerController(String restUrl)
        {
            _client = new RestClient(restUrl);
        }

        public IEnumerable<SummonerModel> SearchForPuuid(String summonerName)
        {
            //sends a GET request to "api/Puuids"
            RestRequest request = new RestRequest($"{summonerName}");
            request.AddHeader("X-Riot-Token", "RGAPI-69794964-ca42-4493-86c3-abf9b5d36e5d" );

            return _client.Get<IEnumerable<SummonerModel>>(request);
        }

    }

}
