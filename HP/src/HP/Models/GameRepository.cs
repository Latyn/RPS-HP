using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HP.Models
{
    public class GameRepository : IGameRepository
    {

        private ApplicationDbContext _context;

        public GameRepository(ApplicationDbContext context)
        {
            _context = context;
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
        public void Result()
        {

        }
    }
}
