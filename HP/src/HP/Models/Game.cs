using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HP.Models
{
    public class Game
    {
        public int Id { get; set; }
        public List<Player> Players{ get; set; }
        public string Winner { get; set; }
    }
}
