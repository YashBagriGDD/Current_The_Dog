using System;
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

        public Platform(string name, Texture2D tex, Rectangle location) : base(name, tex, location)
        {

        }


        protected override void HandleCollisionEnter(object sender, EventArgs e)
        {
        }

        protected override void HandleCollisionExit(object sender, EventArgs e)
        {
        }
    }
}
