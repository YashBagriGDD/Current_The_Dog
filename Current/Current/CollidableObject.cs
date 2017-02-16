using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Current
{
    abstract class CollidableObject : GameObject
    {
        public Collider Coll { get; set; }

        /// <summary>
        /// Make a new Collidable Object and instantiate a collider
        /// </summary>
        /// <param name="texture"></param>
        public CollidableObject() : base()
        {
            throw new NotImplementedException();

        }

        /// <summary>
        /// Make a new Collidable Object and instantiate a collider
        /// </summary>
        /// <param name="texture"></param>
        public CollidableObject(Texture2D texture) : base(texture)
        {
        }

        /// <summary>
        /// What should this object do on a collision?
        /// </summary>
        /// <param name="sender">Who sent this collision?</param>
        /// <param name="e"></param>
        protected abstract void HandleCollision(object sender, EventArgs e);


    }
}
