using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Current
{
    /// <summary>
    /// Abstract class representing an object that can be picked up
    /// </summary>
    abstract class Pickup : CollidableObject
    {
        public int Value { get; set; }

        public Pickup(string name, Texture2D texture, Rectangle location, int value) : base(name, texture, location)
        {
            Value = value;
        }


        public override void Reset()
        {
            Activate();
            base.Reset();
        }


        protected override void HandleCollisionEnter(object sender, EventArgs e)
        {
            Collider other = (Collider)sender;
            if (other.Host is Player)
            {
                //Play Sound effect - common to all pickups
                GameManager.PlaySFX("Pickup");
            }
        }
    }
}
