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
    /// Text to draw on the screen
    /// </summary>
    class UIText : UIObject
    {
        public SpriteFont Font { get; set; }
        public string Text { get; set; }

        public UIText(string name, string text, SpriteFont font, Anchor anchor, SortingMode sortingMode, GameState activeState, Point offset) 
            : base(name, anchor, sortingMode, activeState, offset)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
           // base.Draw(gameTime, spriteBatch);
        }
    }
}
