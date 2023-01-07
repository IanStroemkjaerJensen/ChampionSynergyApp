using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RestSharpClient.Models
{
    public class SummonerModel
    {
        [Display(Name = "Summoner name:")]
        public string? SummonerName { get; set; }
        public string? Puuid { get; set; }
    }
}
