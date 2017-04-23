
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
    class HealthBar : UIObject
    {
        
        //The base UIImage to draw
        protected UIImage baseImage;

        protected Player player; //Reference to the player


        //Constructor taking health texture.
        public HealthBar(string name, Texture2D Texture, Point size) : base(name, Anchor.UpperLeft, SortingMode.None, GameState.Game, Point.Zero)
        {
            baseImage = new UIImage("HealthPiece", Texture, size, Anchor.UpperLeft, SortingMode.Right, GameState.None, Point.Zero, Color.White);

        }

        //Draw method that draws them based on health
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Active)
                return;

            player = GameManager.Get("Current") as Player;


            //Draw as many crossbones as player has health
            for (int i=0; i<player.Health; i++)
            {
                Rectangle loc = new Rectangle(baseImage.Location.X + baseImage.Location.Width * i, baseImage.Location.Y, baseImage.Location.Width, baseImage.Location.Height);
                spriteBatch.Draw(baseImage.Texture, loc, Color.White);
            }

        }
    }
}
