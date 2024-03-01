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
    public class Account
    {
        [Display(Name = "Summoner name:")]
        [JsonPropertyName("gameName")]
        public string? Name { get; set; }

        [Display(Name = "Tagline:")]
        [JsonPropertyName("tagLine")]
        public string? Tagline { get; set; }

        [JsonPropertyName("puuid")]
        public string? Puuid { get; set; }
    }
}