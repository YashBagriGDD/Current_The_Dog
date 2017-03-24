using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Current
{
    class Animate
    {
        Texture2D Texture { get; set; }

        public int Rows { get; set; }

        public int Columns { get; set; }

        public int MillisecondsPerFrame { get; set; }

        private int currentFrame;

        private int totalFrames;

        private int timeSinceLastFrame = 0;


        public Animate(Texture2D texture, int rows, int columns, int millisecondsPerFrame)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            MillisecondsPerFrame = millisecondsPerFrame;
            totalFrames = Rows * Columns;
        }

        public void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if(timeSinceLastFrame > MillisecondsPerFrame)
            {
                currentFrame++;
                timeSinceLastFrame = 0;
                if(currentFrame == totalFrames)
                {
                    currentFrame = 0;
                }
            }
        }

        public void Draw(SpriteBatch spritebatch, Vector2 location)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / Columns);
            int column = currentFrame % Columns;

            // where the character image is being loaded from on the spritesheet
            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);

            // where the character is beng displayed in-game
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            spritebatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}
