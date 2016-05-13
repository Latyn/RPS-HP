using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HP.Models
{
    public class SeedingData
    {
        private ApplicationDbContext _context;

        public SeedingData(ApplicationDbContext context)
        {
            _context = context;
        }

        public void EnsureSeedData()
        {
            if (!_context.Players.Any())
            {
                //Add new Data
                var newPlayer = new Player()
                {
                    Name = "Armando",
                    Chose = Selection.P.ToString(),
                    Score = 6

                };
                _context.Players.Add(newPlayer);

                //Add new Data
                var newPlayer2 = new Player()
                {
                    Name = "Dave",
                    Chose = Selection.S.ToString(),
                    Score=9

                };
                _context.Players.Add(newPlayer2);

                //Add new Data
                var newPlayer3 = new Player()
                {
                    Name = "Richard",
                    Chose = Selection.R.ToString(),
                    Score = 3

                };
                _context.Players.Add(newPlayer3);

                //Add new Data
                var newPlayer4 = new Player()
                {
                    Name = "Michael",
                    Chose = Selection.S.ToString(),
                    Score = 6

                };
                _context.Players.Add(newPlayer4);

                //Add new Data
                var newPlayer5 = new Player()
                {
                    Name = "Allen",
                    Chose = Selection.S.ToString(),
                    Score = 9

                };
                _context.Players.Add(newPlayer5);

                //Add new Data
                var newPlayer6 = new Player()
                {
                    Name = "Omer",
                    Chose = Selection.P.ToString(),
                    Score = 3

                };
                _context.Players.Add(newPlayer6);

                //Add new Data
                var newPlayer7 = new Player()
                {
                    Name = "John",
                    Chose = Selection.R.ToString(),
                    Score = 6

                };
                _context.Players.Add(newPlayer7);

                //Add new Data
                var newPlayer8 = new Player()
                {
                    Name = "Robert",
                    Chose = Selection.P.ToString(),
                    Score = 9

                };
                _context.Players.Add(newPlayer8);

                _context.SaveChanges();

                var newGame = new Game()
                {
                    Players = new List<Player>()
                    {
                        newPlayer,
                        newPlayer2
                    },
                    Winner = newPlayer2.Name
                };

                var newGame2 = new Game()
                {
                    Players = new List<Player>()
                    {
                        newPlayer3,
                        newPlayer4
                    },
                    Winner = newPlayer3.Name
                };
                var newGame3 = new Game()
                {
                    Players = new List<Player>()
                    {
                        newPlayer5,
                        newPlayer6
                    },
                    Winner = newPlayer5.Name
                };
                var newGame4 = new Game()
                {
                    Players = new List<Player>()
                    {
                        newPlayer7,
                        newPlayer8
                    },
                    Winner = newPlayer8.Name
                };
                _context.Games.Add(newGame);
                _context.Games.Add(newGame2);
                _context.Games.Add(newGame3);
                _context.Games.Add(newGame4);
                _context.SaveChanges();

                var tournament = new Tournament()
                {
                    Games = new List<Game>()
                     {
                         newGame,
                         newGame2,
                         newGame3
                     },
                    First = newPlayer.Name,
                    Second = newPlayer2.Name
                };

                _context.Championships.Add(tournament);

                _context.SaveChanges();
            }
        }
    }
}
