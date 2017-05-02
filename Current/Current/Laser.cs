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
    /// The player's laser projectile!
    /// </summary>
    class Laser : CollidableObject
    {

        /// <summary>
        /// The object this is attached to 
        /// </summary>
        GameObject Parent;

        public Laser(string name, Texture2D texture, Rectangle location, GameObject Parent) : base(name, texture, location)
        {
            this.Parent = Parent;
            Deactivate();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Snap to parent location
            if (Active)
            {
                Location = Parent.Location;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }


        protected override void HandleCollisionEnter(object sender, EventArgs e)
        {
        }

        protected override void HandleCollisionExit(object sender, EventArgs e)
        {
        }
    }
}
