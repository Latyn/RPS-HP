using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HP.Models
{
    public class Player
    {
        public string Name { get; set; }
        public int Id { get; set; }

        //Strategy
        public string Chose { get; set; }

    }

    //type of selection 
    public enum Selection
    {
        R = 3,
        S = 2,
        P = 1

    };
}
