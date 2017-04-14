using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Current
{
    class Camera
    {
        ///*
        public Matrix Transform
        {
            get; set;
        }


        Viewport view;

        Vector2 centerview;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newView"> The new viewing position of the camera</param>
        /// <param name="viewportWidth"> The width of the game screen</param>
        /// <param name="viewportHeight"> The height of the game screen</param>
        /// <param name="playerWidth"> The width of the drawn player</param>
        /// <param name="playerHeight"> The height of the drawn player</param>
        public Camera(/*Viewport newView,*/ int viewportWidth, int viewportHeight, int playerWidth, int playerHeight)
        {
            // view = newView;
            // new vector2.X = half the total of the viewport's X value minus half of the player's X value
            // new Vector2.Y = half the total of the viewport's Y value minus half of the player's Y value
            centerview = new Vector2((viewportWidth - (playerWidth / 2)) / 2, (viewportHeight - (playerHeight / 2))/ 2);
        }

        public void Update()
        {
            Transform = Matrix.Identity * Matrix.CreateTranslation(centerview.X, centerview.Y, 0);
        }
        //*/
        
    }
}
