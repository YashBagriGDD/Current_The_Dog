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
    /// Represents a pickup that restores Current's health
    /// </summary>
    class HealthPickup : Pickup
    {
        public HealthPickup(string name, Texture2D texture, Rectangle location, int value) : base(name, texture, location, value)
        {
        }

        protected override void HandleCollisionEnter(object sender, EventArgs e)
        {
            //Include sfx logic
            base.HandleCollisionEnter(sender, e);

            Collider other = (Collider)sender;

            if (other.Host is Player)
            {
                Player p = (Player)other.Host;
                p.Health += Value;
                Deactivate();
            }
        }

        protected override void HandleCollisionExit(object sender, EventArgs e)
        {
            
        }
    }
}
