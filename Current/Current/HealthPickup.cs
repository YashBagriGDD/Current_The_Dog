using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Current
{
    class HealthPickup : Pickup
    {
        public HealthPickup(string name, Texture2D texture, Rectangle location) : base(name, texture, location)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        protected override void HandleCollisionEnter(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void HandleCollisionExit(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
