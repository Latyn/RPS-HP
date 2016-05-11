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
        static Dictionary<string, int> PlayerScores = new Dictionary<string, int>();

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

            List<Tournament> Tournaments = toTournament(JsonText);

            foreach (var tournaments in Tournaments)
            {
                foreach (var game in tournaments.Games)
                {
                    int value = 0;
                    Player winner = Winner(game);
                    // check to see if we need to fetch a court's data
                    if (PlayerScores.ContainsKey(winner.Name) == false)
                    {
                        if (winner.Name != null)
                        {
                            PlayerScores[winner.Name] = value+=1;
                        }
                    }
                }
                var TournamentWiner = PlayerScores.Keys.Max();
                PlayerScores.Clear();
            }         
        }
        private Player Winner(Game game)
        {
                
                
                switch (game.Players[0].Chose)
                {
                    case "R":
                        switch (game.Players[1].Chose)
                        {
                            case "P": return game.Players[1];
                            case "S": return game.Players[0];
                            default: throw new Exception("Logic fail.");
                        }
                    case "S":
                        switch (game.Players[1].Chose)
                        {
                            case "R": return game.Players[1];
                            case "P": return game.Players[0];
                            default: throw new Exception("Logic fail.");
                        }
                    case "P":
                        switch (game.Players[1].Chose)
                        {
                            case "S": return game.Players[1];
                            case "R": return game.Players[0];
                            default: throw new Exception("Logic fail.");
                        }
                default: throw new Exception("Logic fail.");

                }            
        }
        //Method to give the correct structure to Json with no tags
        private List<Tournament> toTournament(string JsonText)
        {
            //Parse Json text to array
            JArray obj = JArray.Parse(JsonText);

            List<Tournament> Tournaments = new List<Tournament>();


            //Checking each level of the array and parsing to a correct structure
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

                        var tempPlayer = _repository.addPlayer(player);
                        Players.Add(tempPlayer);

                    }
                    game.Players = Players;

                    var tempGame = _repository.addGame(game);
                    Games.Add(tempGame);

                }
                tournament.Games = Games;
                Tournaments.Add(tournament);
            }

            return Tournaments;
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
