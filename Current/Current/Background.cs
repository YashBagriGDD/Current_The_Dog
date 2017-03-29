using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Current
{
    class Background : GameObject
    {
        /// <summary>
        /// Speed of the background. (For paralax) 
        /// </summary>
        public float Speed { get; set; } 

        public Background(string name, Texture2D tex, Rectangle location) : base(name, tex, location)
        {

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
