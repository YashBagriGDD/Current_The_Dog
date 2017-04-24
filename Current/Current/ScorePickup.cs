using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Current
{
    class ScorePickup : Pickup
    {
        public ScorePickup(string name, Texture2D texture, Rectangle location, int value) : base(name, texture, location, value)
        {
        }

        /// <summary>
        /// Adds score on collision with player
        /// </summary>
        protected override void HandleCollisionEnter(object sender, EventArgs e)
        {
            //Include sfx logic
            base.HandleCollisionEnter(sender, e);

            Collider other = (Collider)sender;

            if (other.Host is Player)
            {
                GameManager.Score += Value;
                Deactivate(); //Don't let this be picked up again
            }
        }

        protected override void HandleCollisionExit(object sender, EventArgs e)
        {

        }
    }
}
