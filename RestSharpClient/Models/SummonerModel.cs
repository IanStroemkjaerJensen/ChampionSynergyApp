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
        public string? name { get; set; }
        public string? puuid { get; set; }
    }
}
