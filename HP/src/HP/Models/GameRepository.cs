using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
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

        public string readFile(string ResultUrl)
        {
            
            var result = "";

            if (File.Exists(ResultUrl))
            {
                result = File.ReadAllText(ResultUrl, Encoding.ASCII);
            }

            return result;
        }
        public Player addPlayer(Player player)
        {
            _context.Players.Add(player);
            _context.SaveChanges();
            return player;
        }
        public Game addGame(Game game)
        {
            _context.Games.Add(game);
            _context.SaveChanges();
            return game;
        }
        public Tournament addTournament(Tournament tournament)
        {
            _context.Championships.Add(tournament);
            _context.SaveChanges();
            return tournament;
        }
    }
}
