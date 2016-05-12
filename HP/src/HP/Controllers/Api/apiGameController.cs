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

        //To check winner in a tournament
        [HttpGet("api/championship/tournament")]
        public JsonResult Tournament()
        {
            var JsonText = _repository.Result();

            Player winner = _repository.Result(JsonText);

            //Json now as a part of the father controller class

            string[] text = new string[2] { winner.Name, winner.Chose };

            return Json(text);
        }


        [HttpPost("/api/championship/result")]
        //Example /?first=Dave&second=Armando
        public JsonResult Result(string first, string second)
        {
            Player firstPlayer = new Player();
            Player secondPlayer = new Player();


            //Based on what the solution is asking for there is not a score value to  be saved, the score is initialized as 0
            firstPlayer.Name = first;
            firstPlayer.Score = 0;
            secondPlayer.Name = second;
            secondPlayer.Score = 0;

            _repository.addPlayer(firstPlayer);
            _repository.addPlayer(secondPlayer);

            return Json(new { status = "success" });

        }
        [HttpPost("/api/championship/clear")]
        //Example /?first=Dave&second=Armando
        public void Clear(string first, string second)
        {
            _repository.ClearDB();
        }
        [HttpPost("/api/championship/multiple")]
        public JsonResult Multiple(string data)
        {

            //var JsonText = _repository.Result();

            List<Tournament> Tournaments = _repository.toTournament(data);

            _repository.CheckTournament(Tournaments);

            return Json(new { status = "success" });
        }

        [HttpPost("/api/championship/new")]
        public JsonResult New(string data)
        {
            List<Tournament> Tournaments = new List<Tournament>();
            Tournament Tournament = new Tournament();
            //var JsonText = _repository.Result();

            Game game = _repository.toGame(data);
            Tournament.Games = new List<Game>()
                     {
                         game
                     };
            Tournaments.Add(Tournament);
            _repository.CheckTournament(Tournaments);

            var player = _repository.checkPlayerByName(Tournament.First);
            string[] win = new string[2] { player.Name, player.Chose };

            return Json(new { winner = win });
        }


        //method algorithm that takes a two-element list
        //return the full player object (name and chose(strategy)) of the winning player
        private Player Winner(Game game)
        {
            //If the number of players is not equal to 2, raise an exception.
            if (game.Players.Count > 2)
            {
                throw new Exception("Cannot have more than two players.");
            }
            else
            {
                switch (game.Players[0].Chose)
                {
                    case "R":
                        switch (game.Players[1].Chose)
                        {
                            case "P": return game.Players[1];
                            case "S": return game.Players[0];
                            case "R": return game.Players[0];//In case of draw we return the first player
                            default: throw new Exception("Logic fail.");
                        }
                    case "S":
                        switch (game.Players[1].Chose)
                        {
                            case "R": return game.Players[1];
                            case "P": return game.Players[0];
                            case "S": return game.Players[0];//In case of draw we return the first player
                            default: throw new Exception("Logic fail.");
                        }
                    case "P":
                        switch (game.Players[1].Chose)
                        {
                            case "S": return game.Players[1];
                            case "R": return game.Players[0];
                            case "P": return game.Players[0];//In case of draw we return the first player
                            default: throw new Exception("Logic fail.");
                        }
                    default: throw new Exception("Logic fail.");

                }
            }
           
        }
    }
}
