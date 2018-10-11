using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGameWinform.Models
{
    [Serializable]
    class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string PartnerUserName { get; set; }
        public int Score { get; set; }
    }
}
