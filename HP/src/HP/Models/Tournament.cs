using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.Models
{
    public  class Tournament
    {
        public int Id { get; set; }
        public List<Game> Games { get; set; }
        public string First { get; set; }
        public string Second { get; set; }
        public int FirstPlaceScore { get; set; }
        public int SecondPlaceScore { get; set; }

    }
}
