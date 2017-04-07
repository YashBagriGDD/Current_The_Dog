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
            Collider other = (Collider)sender;

            if (other.Host is Player)
            {
                //TODO 
            }
        }

        protected override void HandleCollisionExit(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
