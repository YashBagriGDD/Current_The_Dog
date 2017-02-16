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
        public int Score { get; set; }
        public float Time { get; set; }
        public SpriteFont Font{ get; set; }

        public HUD(SpriteFont font) : base()
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
