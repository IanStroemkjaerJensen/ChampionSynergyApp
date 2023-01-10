﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestSharpClient.Models
{
    public class Info
    {
        [JsonPropertyName("participants")]
        public List<Participant>? Participants { get; set; }

    }
}
