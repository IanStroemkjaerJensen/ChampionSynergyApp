using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestSharpClient.Models
{
    public class Participant
    {
        /* Deprecated
        [JsonPropertyName("summonerName")]
        public string? SummonerName { get; set; }
        */
        [JsonPropertyName("riotIdGameName")]
        public string? riotIdGameName { get; set; }

        [JsonPropertyName("teamId")]
        public int? TeamId { get; set; }

        [JsonPropertyName("championName")]
        public string? ChampionName { get; set; }

        [JsonPropertyName("win")]
        public Boolean? Win { get; set; }

        [JsonPropertyName("teamPosition")]
        public string? TeamPosition { get; set; }
    }
}
