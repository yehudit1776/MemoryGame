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

        public Game()
        {
            CardArray = new Dictionary<string, string>();
            int i = 0;
            while (i < 9)
            {
                int numCard = new Random().Next(1, 100);
                if (CardArray.Keys.Contains(numCard.ToString()))
                    continue;
                CardArray.Add(numCard.ToString(), null);
                i++;
            }
        }
    }
}
