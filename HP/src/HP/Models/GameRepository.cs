using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Data.Entity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.Models
{
    public class GameRepository : IGameRepository
    {

        private ApplicationDbContext _context;
        private IHostingEnvironment _host;
        static Dictionary<string, int> PlayerScores = new Dictionary<string, int>();

        public GameRepository(ApplicationDbContext context, IHostingEnvironment host)
        {
            _context = context;
            _host = host;
        }

        public IEnumerable<string> GetTop(int count)
        {
            if (count == 0)
            {
                return _context.Championships.Select(m => m.First).ToList();
            }
            else
            {
                return _context.Championships.Select(m => m.First).Take(count).ToList();
            }

        }
        public void New()
        {
             
        }
        public string Result()
        {
            var test = _host.WebRootPath;
            var ResultUrl =test + Startup.Configuration["AppSettings:Url"];
            var JsonText = readFile(ResultUrl);

            return JsonText;

        }

        //Receive a torunament and give us the winner
        public Player Result(string EncodedArray)
        {
            List<Tournament> Tournaments = toTournament(EncodedArray);
            Player winnerPlayer = new Player();
            Player secondPlayer = new Player();

            int value = 0;

            foreach (var tournaments in Tournaments)
            {
                foreach (var game in tournaments.Games)
                {
                    
                    Player winner = Winner(game);
                    // check to see if we need to fetch a court's data
                    if (PlayerScores.ContainsKey(winner.Name) == false)
                    {
                        value = 0;
                        if (winner.Name != null)
                        {
                            PlayerScores[winner.Name] = value += 1;
                        }
                    }
                    else
                    {
                        PlayerScores[winner.Name] = value += 1;
                    }
                }

                winnerPlayer = GetHighest();
                secondPlayer = GetHighest();
                setPoints(tournaments, winnerPlayer, secondPlayer);
                PlayerScores.Clear();
            }

            return winnerPlayer;

        }
        //Checks first and second place in a tournament
        public void CheckTournament(List<Tournament> Tournaments)
        {
            Player winnerPlayer = new Player();
            Player secondPlayer = new Player();
            int value = 0;

            foreach (var tournaments in Tournaments)
            {
                foreach (var game in tournaments.Games)
                {
                   //Checks game winner
                    Player winner = Winner(game);
                    // check to see if we need to fetch a court's data
                    if (PlayerScores.ContainsKey(winner.Name) == false)
                    {
                        value = 0;
                        if (winner.Name != null)
                        {
                            PlayerScores[winner.Name] = value += 1;
                        }
                    }
                    else
                    {
                        PlayerScores[winner.Name] = value += 1;
                    }
                }
                winnerPlayer = GetHighest();

                if (PlayerScores.Count!=0)
                {
                    secondPlayer = GetHighest();
                }

                setPoints(tournaments, winnerPlayer, secondPlayer);

                PlayerScores.Clear();
                //ClearDB();
            }
        }

        //Get highest player from tournament
        private Player GetHighest()
        {
            Player highestPlayer = new Player();
            var highest="";
            var score = 0;

            foreach (var item in PlayerScores)
            {
                if (PlayerScores.Values.Max() == item.Value)
                {
                    highest = item.Key;
                    score = item.Value;
                }
            }

            highestPlayer = _context.Players.Where(m => m.Name == highest).First();
            highestPlayer.Score = score;

            _context.SaveChanges();

            PlayerScores.Remove(highest);

            return highestPlayer;
        }

        //Read File from URL
        public string readFile(string ResultUrl)
        {
            
            var result = "";

            if (File.Exists(ResultUrl))
            {
                result = File.ReadAllText(ResultUrl, Encoding.ASCII);
            }

            return result;
        }

        //add Player and save changes
        public Player addPlayer(Player player)
        {
            _context.Players.Add(player);
            _context.SaveChanges();
            return player;
        }

        //Add Games and save changes
        public Game addGame(Game game)
        {
            _context.Games.Add(game);
            _context.SaveChanges();
            return game;
        }

        //add Tournament and save changes
        public Tournament addTournament(Tournament tournament)
        {
            _context.Championships.Add(tournament);
            _context.SaveChanges();
            return tournament;
        }

        //Method to give the correct structure to Json with no tags, multiple tournaments
        public List<Tournament> toTournament(string JsonText)
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

                        var tempPlayer = addPlayer(player);
                        Players.Add(tempPlayer);

                    }
                    game.Players = Players;

                    var tempGame = addGame(game);
                    Games.Add(tempGame);

                }
                tournament.Games = Games;
                Tournaments.Add(tournament);

            }

            return Tournaments;
        }

        //Method to give the correct structure to Json with no tags, games in a single tournament
        public Game toGame(string JsonText)
        {
            //Parse Json text to array
                JArray obj = JArray.Parse(JsonText);
                Game game = new Game();
                List<Player> Players = new List<Player>();

                 foreach (var item2 in obj)
                {                   
                        var player = new Player();
                        player.Name = item2[0].ToString();
                        player.Chose = item2[1].ToString();

                        var tempPlayer = addPlayer(player);
                        Players.Add(tempPlayer);
                }

            game.Players = Players;

            var tempGame = addGame(game);
            return tempGame;
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
        private void setPoints(Tournament mytournament, Player winner, Player second)
        {

            mytournament.FirstPlaceScore = 3;
            mytournament.SecondPlaceScore = 1;
            mytournament.First = winner.Name;
            mytournament.Second = second.Name;

            _context.Championships.Add(mytournament);

            _context.SaveChanges();
        }

        public Player checkPlayerByName(string name)
        {
           return _context.Players.Where(m => m.Name == name).First();
        }
        public void ClearDB()
        {
            //I made this in this way due I am working with entity framework 7 and it seems to have just a new version so bindings and delete cascade is not working properly 

            _context.Players.ToList().ForEach(p => _context.Players.Remove(p));
            _context.SaveChanges();

            _context.Championships.ToList().ForEach(p => _context.Games.ToList().ForEach(e => _context.Games.Remove(e)));
            _context.SaveChanges();

            _context.Championships.ToList().ForEach(p => _context.Championships.Remove(p));
            _context.SaveChanges();

        }
    }
}
