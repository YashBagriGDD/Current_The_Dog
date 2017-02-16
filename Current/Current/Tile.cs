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
        public Tile(Texture2D texture) : base(texture)
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

        protected override void HandleCollision(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
