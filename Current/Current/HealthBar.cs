
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Current
{
    /// <summary>
    /// Represents the health bar
    /// </summary>
    class HealthBar : GameObject
    {
        
        //The base UIImage to draw
        protected UIImage baseImage;

        protected Player player; //Reference to the player


        //Constructor taking health texture.
        public HealthBar(string name, Texture2D Texture, Point size) : base(name, Rectangle.Empty)
        {
            baseImage = new UIImage("HealthPiece", Texture, size, Anchor.UpperLeft, SortingMode.Right, GameState.None, Point.Zero, Color.White);

            player = GameManager.Get("Current") as Player;
        }

        //Draw method that draws them based on health
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!InTheRightState)
                return;

            //Draw as many crossbones as player has health
            for (int i=0; i<player.Health; i++)
            {
                Rectangle loc = new Rectangle(baseImage.Location.X + baseImage.Location.Width * i, baseImage.Location.Y, baseImage.Location.Width, baseImage.Location.Height);
                spriteBatch.Draw(baseImage.Texture, loc, Color.White);
            }

        }
    }
}
