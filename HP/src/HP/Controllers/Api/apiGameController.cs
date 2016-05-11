using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HP.Models;
using System.IO;
using Newtonsoft.Json.Linq;

namespace HP.Controllers.Api
{
    public class ApiGameController:Controller
    {
        private IGameRepository  _repository;

        public ApiGameController(IGameRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet ("api/championship/top")]
        public JsonResult Top( int count )
        {
            string[] tops  = _repository.GetTop(count).ToArray();

            //Json now as a part of the father controller class
            return Json(new { players    = tops });
        }
        
        [HttpPost("/api/championship/result")]
        public void Result()
        {

            var JsonText = _repository.Result();
            var Object = Json(JsonText);

            JArray obj = JArray.Parse(JsonText);

            List<Tournament> Tournaments = new List<Tournament>();

            foreach (var item in obj)
            {
                var tournament = new Tournament();
                List<Game> Games = new List<Game>();

                foreach (var item2 in item)
                {
                    var game = new Game();
                    List<Player> Players = new List<Player>();
                    foreach (var item3 in item2)
                    {
                        var player = new Player();
                        player.Name = item3[0].ToString();
                        player.Chose = item3[1].ToString();

                        var tempPlayer =_repository.addPlayer(player);
                        Players.Add(tempPlayer);
                                             
                    }
                    game.Players = Players;

                    var tempGame = _repository.addGame(game);
                    Games.Add(tempGame);

                    
                }
                tournament.Games = Games;
                Tournaments.Add(tournament);
            }


        }


        [HttpPost("/api/championship/New")]
        public IActionResult New()
        {

            //Json now as a part of the father controller class
            return Json("test");
        }

    }
    [Newtonsoft.Json.JsonObject(Title = "Players")]
    public class Test
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "name")]
        public List<string> Name { get; set; }

    }
}
