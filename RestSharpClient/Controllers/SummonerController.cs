using RestSharp;
using RestSharpClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            return _client.Get<IEnumerable<SummonerModel>>(request);
        }

    }

}
