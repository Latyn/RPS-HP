using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HP.Models
{
    public class Player
    {
        //[JsonProperty("")]
        public string Name { get; set; }

        [JsonIgnore]
        public int Id { get; set; }

        //[JsonProperty("")]
        //Strategy
        public string Chose { get; set; }

        [JsonIgnore]
        public int Score { get; set; }

    }

    //type of selection 
    public enum Selection
    {
        R = 3,
        S = 2,
        P = 1

    };
}
