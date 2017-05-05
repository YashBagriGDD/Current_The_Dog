using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Current
{
    class LaserPickup : Pickup
    {
        public LaserPickup(string name, Texture2D texture, Rectangle location, int value) : base(name, texture, location, value)
        {
        }


        protected override void HandleCollisionEnter(object sender, EventArgs e)
        {
            base.HandleCollisionEnter(sender, e);
            Collider other = (Collider)sender;
            if (other.Host is Player)
            {
                //Turn on laser ability for player
                Player p = (Player)(other.Host);
                p.HasLaser = true;
                Deactivate();
            }
        }

        protected override void HandleCollisionExit(object sender, EventArgs e)
        {
        }
    }
}
