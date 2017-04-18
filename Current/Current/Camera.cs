using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Current
{
    /// <summary>
    /// Camera class.
    /// Some inspiration taken from https://roguesharp.wordpress.com/2014/07/13/tutorial-5-creating-a-2d-camera-with-pan-and-zoom-in-monogame/
    /// </summary>
    class Camera : GameObject
    {
        /// <summary>
        /// Matrix used to offset everything we draw
        /// </summary>
        public Matrix TransformMatrix
        {
            get
            {
                //Matrix multiplication is commutative, so from bottom to top:
                //We center the camera based on the screen, and then offset by -camera location
                //in order to simulate camera movement.
                return Matrix.CreateTranslation(-Location.X, -Location.Y, 0) *
                    Matrix.CreateTranslation(Game1.TargetWidth/2, Game1.TargetHeight/2, 0);
            }
        }


        /// <summary>
        /// Who should the camera look at?
        /// </summary>
        public GameObject Target { get; set; }
        
        /// <summary>
        /// Create a new Camera
        /// </summary>
        /// <param name="name">Name of the camera</param>
        /// <param name="location">Location of the camera - typically set to empty rectangle</param>
        /// <param name="target">GameObject to look at</param>
        public Camera(string name, Rectangle location, GameObject target) : base(name, location )
        {
            Target = target;
        }

        /// <summary>
        /// Set location to that of the target
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            Location = Target.Location;
        }

        /// <summary>
        /// Don't draw anything
        /// </summary>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
        }

    }
}
