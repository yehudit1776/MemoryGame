using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGameWinform.Models
{[Serializable]
    public class HelpCardsAndCurrentTurn
    {
        public string CurrentTurn { get; set; }
        public Dictionary<string, string> CardList { get; set; }
    }
}
