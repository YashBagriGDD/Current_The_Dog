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
        private static Dictionary<string, GameObject> Objects { get; }
            = new Dictionary<string, GameObject>();
        public static List<CollidableObject> CollidableObjects { get; }
            = new List<CollidableObject>();
        public static int Score { get; set; }
        public static float Time { get; set; }


        public static void Add(string name, GameObject g)
        {
            Objects.Add(name, g);
            if (g is CollidableObject)
            {
                CollidableObject c = (CollidableObject)g;
                CollidableObjects.Add(c);
            }
                
        }

        public static GameObject Get(string name)
        {
            return Objects[name];
        }

        public static Dictionary<string, GameObject> GetAll()
        {
            return Objects;
        }
   
    }
}
