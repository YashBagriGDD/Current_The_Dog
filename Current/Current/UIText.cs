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

        //Construct a new UIText object
        public UIText(string name, string text, SpriteFont font, Anchor anchor, SortingMode sortingMode, GameState activeState, Point offset, Color color) 
            : base(name, anchor, sortingMode, activeState, offset)
        {
            //Store info
            Font = font;
            Text = text;
            DrawColor = color;

            //Calculate width and height
            Vector2 fontSize = font.MeasureString(text);
            Location.Width = (int)fontSize.X;
            Location.Height = (int)fontSize.Y;

            //Anchor it
            UIManager.AddAnchoredObject(this, anchor, sortingMode, offset);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (GameManager.gameState != ActiveState)
                return;

            spriteBatch.DrawString(Font, Text, new Vector2(Location.X, Location.Y), DrawColor);
        }
    }
}
