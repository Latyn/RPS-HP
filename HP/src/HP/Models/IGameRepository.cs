using System.Collections.Generic;

namespace HP.Models
{
    public interface IGameRepository
    {
        IEnumerable<string> GetTop(int count);
        void New();
        void Result();
    }
}