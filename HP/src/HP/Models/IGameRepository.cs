using System.Collections.Generic;

namespace HP.Models
{
    public interface IGameRepository
    {
        IEnumerable<string> GetTop(int count);
        void New();
        string Result();
        Player addPlayer(Player player);
        Game addGame(Game player);
        Tournament addTournament(Tournament player);
    }
}