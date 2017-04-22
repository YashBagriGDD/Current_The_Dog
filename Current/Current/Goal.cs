using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Current
{
    /// <summary>
    /// Represents the goal of the level
    /// </summary>
    class Goal : Pickup
    {
        public Goal(string name, Texture2D tex, Rectangle location) : base(name, tex, location, 0)
        {

        }


        protected override void HandleCollisionEnter(object sender, EventArgs e)
        {
            Collider other = (Collider)sender;
            if (other.Host is Player)
            {

            }
        }

        protected override void HandleCollisionExit(object sender, EventArgs e)
        {
        }
    }
}
