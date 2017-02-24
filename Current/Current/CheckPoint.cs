using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Current
{
    class CheckPoint : CollidableObject
    {
        /// <summary>
        /// unique ID for this checkpoint. Read-only
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// Makes a checkpoint AND generates a unique ID
        /// </summary>
        /// <param name="tex"></param>
        public CheckPoint(string name, Texture2D tex) : base(name, tex)
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

        protected override void HandleCollision(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
