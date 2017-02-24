using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Current
{
    class Collider
    {
        /// <summary>
        /// GameObject this is attached to
        /// </summary>
        public GameObject Host { get; set; }

        public float Gravity { get; set; }

        public bool Solid { get; set; }

        public Rectangle hitbox { get; } 

        /// <summary>
        /// Type of Collider. 
        /// This should probably its own data type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The Collision EventHandler.
        /// Call OnCollision when there is a collision, and the event will fire!
        /// </summary>
        public event EventHandler Collision;


        public Collider(GameObject host)
        {
            Host = host;
          
            hitbox = new Rectangle((int)Host.Position.X, (int)Host.Position.Y, Host.Texture.Width, Host.Texture.Height);
            
        }

        /// <summary>
        /// Calls the Collision event
        /// </summary>
        public void OnCollision()
        {
            if (Collision != null)
            {
                Collision(this, EventArgs.Empty);
            }
        }

    }
}
