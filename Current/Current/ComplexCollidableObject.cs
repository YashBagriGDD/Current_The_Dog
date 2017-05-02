using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Current
{
    /// <summary>
    /// Like CollidableObject, but contains 4 additional colliders for more complex detection.
    /// </summary>
    abstract class ComplexCollidableObject : CollidableObject
    {
        //These colliders are used for detecting collisions above, below, left, and right of objects
        protected  Collider CollAbove { get; set; }
        protected Collider CollBelow { get; set; }
        protected Collider CollLeft { get; set; }
        protected Collider CollRight { get; set; }

        /// <summary>
        /// Draw rectangles where the colliders are?
        /// </summary>
        protected bool DebugDraw { get; set; } 

        /// <summary>
        /// Makes a new object with colliders above, below, left, and right
        /// </summary>
        /// <param name="name"></param>
        /// <param name="texture"></param>
        /// <param name="location"></param>
        public ComplexCollidableObject(string name, Texture2D texture, Rectangle location) : base(name, texture, location)
        {
            //Multipliers for the longer and shorter dimensions of the rectangle
            float maxMod = .35f;
            float minMod = .10f;
            //The width and height of the new horizontal rectangles
            int hWidth = (int)(Location.Width * maxMod);
            int hHeight = (int)(Location.Height * minMod);
            //The width and height of the new vertical rectangles
            int vWidth = (int)(Location.Width * minMod);
            int vHeight = (int)(Location.Height * maxMod);

            //Construct rectangles centered at the top, bottom, left, and right of object.
            //Note: (0,0) is replaced with (Location.X, Location.Y) in these calculations because 
            //The Collider constructor assumes we're providing an offset
            Rectangle rAbove = new Rectangle((int)((.5f * Location.Width - .5f * hWidth)), 0,
                hWidth, hHeight);
            Rectangle rBelow = new Rectangle((int)(.5f * Location.Width - .5f * hWidth), Location.Height - hHeight, 
                hWidth, hHeight);
            Rectangle rLeft = new Rectangle(0, (int)(.5f*Location.Height - .5f*vHeight),
                vWidth, vHeight);
            Rectangle rRight = new Rectangle(Location.Width-vWidth, (int)(.5f * Location.Height - .5f * vHeight),
                vWidth, vHeight);

            //Actually make the colliders
            CollAbove = new Collider(this, rAbove);
            CollBelow = new Collider(this, rBelow);
            CollLeft = new Collider(this, rLeft);
            CollRight = new Collider(this, rRight);

            DebugDraw = false;


        }

        public override void Update(GameTime gameTime)
        {
            //Update the rest of the colliders as well
            CollAbove.Update(gameTime);
            CollBelow.Update(gameTime);
            CollLeft.Update(gameTime);
            CollRight.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            if (DebugDraw)
            {
                spriteBatch.Draw(Game1.Textures["WhiteBlock"], CollAbove.Hitbox, Color.White);
                spriteBatch.Draw(Game1.Textures["WhiteBlock"], CollBelow.Hitbox, Color.White);
                spriteBatch.Draw(Game1.Textures["WhiteBlock"], CollRight.Hitbox, Color.White);
                spriteBatch.Draw(Game1.Textures["WhiteBlock"], CollLeft.Hitbox, Color.White);
            }



        }
    }
}
