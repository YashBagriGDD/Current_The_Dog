﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Current
{
    class Platform : CollidableObject
    {

        public Platform(string name, Texture2D tex) : base(name, tex)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
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
