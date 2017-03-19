using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Current
{
    /// <summary>
    /// Static class with helper functions to map keys together.
    /// </summary>
    static class InputManager
    {
        public static Dictionary<string, Keys[]> KeysDictionary { get; private set; }

        private static KeyboardState prev;

        /// <summary>
        /// Setup the InputManager
        /// </summary>
        public static void Init()
        {
            KeysDictionary = new Dictionary<string, Keys[]>
            {
                {"Right", new Keys[] {Keys.D, Keys.Right } },
                {"Left", new Keys[] {Keys.A, Keys.Left} },
                {"Jump", new Keys[] {Keys.Space} },
                {"Up", new Keys[] {Keys.W, Keys.Up} },
                {"Down", new Keys[] {Keys.D, Keys.Down} }
            };

            prev = Keyboard.GetState();

        }
        
        /// <summary>
        /// Is the button currently pressed?
        /// </summary>
        /// <param name="button">button name</param>
        /// <returns></returns>
        public static bool GetButton(string button)
        {
            var ks = Keyboard.GetState();
            foreach (Keys k in KeysDictionary[button])
            {
                if (ks.IsKeyDown(k))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// WAS the button pressed last frame?
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static bool GetButtonDown(string button)
        {
            var ks = Keyboard.GetState();
            foreach (Keys k in KeysDictionary[button])
            {
                if (ks.IsKeyDown(k) && prev.IsKeyUp(k))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool GetButtonUp(string key)
        {
            throw new NotImplementedException();
        } 
        
        public static void Update(GameTime gameTime)
        {
            var ks = Keyboard.GetState();

            prev = Keyboard.GetState();
        }

    }
}
