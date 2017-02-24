using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Current
{
    /// <summary>
    /// Manages the game.
    /// </summary>
    static class GameManager
    {
        /// <summary>
        /// This should hold ALL GameObjects in the game. 
        /// </summary>
        public static Dictionary<string, GameObject> Objects { get; }
            = new Dictionary<string, GameObject>();
        public static int Score { get; set; }
        public static float Time { get; set; }

   
    }
}
