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

        public GameObject Parent { get; set; }
        
        public string Name { get; set; }

        private int currentFrame;

        private int totalFrames;

        private int timeSinceLastFrame = 0;

        private bool isRunning = false;

        public static int ONESIXTIETHSECPERFRAME = (int)( (1/60.0f) * 1000);



        public Animate(Texture2D texture, int rows, int columns, int millisecondsPerFrame, GameObject parent)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            MillisecondsPerFrame = millisecondsPerFrame;
            totalFrames = Rows * Columns;
            Parent = parent;
            Name = GameManager.TrimFilePath(texture.Name);
            isRunning = true;
        }
        
        /// <summary>
        /// Resets the current time of the animation
        /// </summary>
        public void Reset()
        {
            currentFrame = 0;
            timeSinceLastFrame = 0;

        }

        /// <summary>
        /// Pause the animation
        /// </summary>
        public void Pause()
        {
            Reset();
            isRunning = false;
        }

        /// <summary>
        /// Resume the animation
        /// </summary>
        public void Resume()
        {
            Reset();
            isRunning = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!isRunning)
                return;
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

        public void Draw(SpriteBatch spriteBatch)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / Columns);
            int column = currentFrame % Columns;

            // where the character image is being loaded from on the spritesheet
            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);

            // where the character is beng displayed in-game
            Rectangle destinationRectangle = new Rectangle(Parent.Location.X, Parent.Location.Y, Parent.Location.Width, Parent.Location.Height);
            
            //spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);


            spriteBatch.Draw(Texture, destinationRectangle: destinationRectangle, sourceRectangle: sourceRectangle, color: Parent.DrawColor, effects: Parent.SpriteFX);


        }
    }
}
