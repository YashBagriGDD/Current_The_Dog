using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Current
{
    // the actual data being saved by the GameSaver Class
    class SavedData
    {
        public int score { get; set; }
        public string name { get; set; }
        public int progress { get; set; }

        public int highscore { get; set; }

        public SavedData(int sc , string nm, int pr, int hs)
        {
            score = sc;
            name = nm;
            progress = pr;
            highscore = hs;
        }

    }
}
