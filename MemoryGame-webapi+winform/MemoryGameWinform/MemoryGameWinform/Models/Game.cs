using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGameWinform.Models
{
    public class Game
    {
        public static List<Game> Games;
        private User Player1;
        private User Player2;
       
        public string CurrentTurn { get; set; }
        public Dictionary<string, string> CardArray { get; set; }

      
    }
}
