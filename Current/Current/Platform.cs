using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Current
{
    class Platform : CollidableObject
    {
        bool collided = false;
        public Platform(string name, Texture2D tex) : base(name, tex)
        {
            //throw new NotImplementedException();

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Rectangle(5, 5, 100, 100), Color.White);
            if (collided)
                spriteBatch.Draw(Texture, new Rectangle(100, 5, 100, 100), Color.White);

        }

        public override void Update(GameTime gameTime)
        {
            
        }

        protected override void HandleCollision(object sender, EventArgs e)
        {
            collided = true;
        }
    }
}
