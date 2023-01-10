using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RestSharpClient.Models
{
    public class Summoner
    {
        [Display(Name = "Summoner name:")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("puuid")]
        public string? Puuid { get; set; }
    }
}
