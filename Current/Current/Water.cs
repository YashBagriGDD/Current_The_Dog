using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Current
{
    class Water : Volume
    {
        public Water(string name, Texture2D tex, Rectangle location, Vector2 force) : base(name, tex, location, force)
        {
            DrawColor = Color.Blue;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }



        protected override void HandleCollisionEnter(object sender, EventArgs e)
        {
        }

        protected override void HandleCollisionExit(object sender, EventArgs e)
        {
        }
    }
}
