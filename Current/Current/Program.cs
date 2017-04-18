using System;

namespace Current
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    static class Program
    {

        /// <summary>
        /// The actual game object
        /// </summary>
        public static Game1 GAME; 

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            GAME = new Game1();
            using (GAME)
                GAME.Run();
        }
    }
#endif
}
