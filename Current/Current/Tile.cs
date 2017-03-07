using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Current
{
    class Tile : CollidableObject
    {
        public Tile(string name, Texture2D texture, Rectangle location) : base(name, texture, location)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void HandleCollisionEnter(object sender, EventArgs e)
        {

        }

        protected override void HandleCollisionExit(object sender, EventArgs e)
        {

        }
    }
}
