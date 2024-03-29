﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestSharpClient.Models
{
    public class Teammate
    {    
        public string? SummonerName { get; set; }
     
        public int? TeamId { get; set; }
   
        public string? ChampionName { get; set; }
   
        public Boolean? Win { get; set; }

        public string Role { get; set; }

        public int? Wins { get; set; } 

        public int? Losses { get; set; }
        
        public int? Played { get; set; }    
    }
}
