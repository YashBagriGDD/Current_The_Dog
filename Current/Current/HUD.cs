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
    /// The Heads-Up-Display
    /// </summary>
    class HUD : GameObject
    {

        public SpriteFont Font{ get; set; }

        public HUD(string name, Rectangle location, SpriteFont font) : base(name, location)
        {
            Font = font;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
