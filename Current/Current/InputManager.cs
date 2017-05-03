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

        //This private field is here so the Point doesn't have to be recreated many times per second.
        private static Point mousePos = Point.Zero;
        /// <summary>
        /// Get the current Mouse position as a Point
        /// </summary>
        public static Point MousePos {
            get
            {
                float xMult = ((float)Game1.TargetWidth / Program.GAME.WindowSize.X);
                float yMult = ((float)Game1.TargetWidth / Program.GAME.WindowSize.X);


                mousePos.X = (int)(Mouse.GetState().X * xMult);
                mousePos.Y = (int)(Mouse.GetState().Y * yMult);
                return mousePos;
            }
        }

        private static KeyboardState prev;
        private static MouseState prevMouse;


        /// <summary>
        /// Setup the InputManager
        /// </summary>
        public static void Init()
        {
            KeysDictionary = new Dictionary<string, Keys[]>
            {
                {"Right", new Keys[] {Keys.D, Keys.Right } },
                {"Left", new Keys[] {Keys.A, Keys.Left} },
                {"Up", new Keys[] {Keys.W, Keys.Up} },
                {"Down", new Keys[] {Keys.S, Keys.Down} },
                {"Jump", new Keys[] {Keys.Space, Keys.X} },
                {"Fire", new Keys[] {Keys.LeftShift, Keys.Z} },
                {"Cancel", new Keys[] {Keys.Escape, Keys.Back } },
                {"Fullscreen", new Keys[] {Keys.Enter} }
            };


            prev = Keyboard.GetState();
            prevMouse = Mouse.GetState();

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
        public static bool GetButtonUp(string button)
        {
            var ks = Keyboard.GetState();

            foreach (Keys k in KeysDictionary[button])
            {
                if (ks.IsKeyUp(k) && prev.IsKeyDown(k))
                {
                    return true;
                }
            }
            return false;
        } 

        /// <summary>
        /// Returns true the first frame LMB down
        /// </summary>
        /// <returns></returns>
        public static bool MouseClickedDown()
        {
            var ms = Mouse.GetState();
            if (ms.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released)
            {
                return true;
            }
            return false;
        }
        
        public static void Update(GameTime gameTime)
        {
            prev = Keyboard.GetState();
            prevMouse = Mouse.GetState();
        }

    }
}
