using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Current
{
    /// <summary>
    /// Text drawn in world space, just like typical game objects. (I.E. it moves with the camera)
    /// </summary>
    class WorldText : GameObject
    {

        /// <summary>
        /// Actual text to display
        /// </summary>
        public string Text { get; set; }

        public SpriteFont Font { get; set; }

        /// <summary>
        /// Strings drawn with vector2, not rectangle
        /// </summary>
        private Vector2 position;

        public WorldText(string name, string text, SpriteFont font, Vector2 position) : base(name, new Rectangle((int)position.X, (int)position.Y, 0,0))
        {
            Text = text;
            Font = font;
            //It's in the world, so we can assume this is only drawn in the game state.
            ActiveState = GameState.Game;

            this.position = position;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Active)
            {
                //Update position since it's a vector2 and we're not drawing with the regular rectangle 
                position.X = Location.X;
                position.Y = Location.Y;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Active)
            {
                spriteBatch.DrawString(Font, Text, position, DrawColor);
            }
        }
    }
}
