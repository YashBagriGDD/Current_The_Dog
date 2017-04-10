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
    /// <summary>
    /// A Collidable version of GameObject.
    /// NOTE: HandleCollisionEnter and Exit will NOT be called if Active is false!
    /// Likewise, Active objects colliding with inactive objects will not result in collisions.
    /// </summary>
    abstract class CollidableObject : GameObject
    {
        //The Collider associated with this object
        public Collider Coll { get; set; }


        /// <summary>
        /// Make a new Collidable Object and instantiate a collider
        /// </summary>
        /// <param name="texture"></param>
        public CollidableObject(string name, Texture2D texture, Rectangle location) : base(name, texture, location)
        {
            Collider collider = new Collider(this);
            Coll = collider;

            //Subscribe to events
            Coll.CollisionEnter += HandleCollisionEnter;
            Coll.CollisionExit += HandleCollisionExit;

        }

        /// <summary>
        /// Update method implementation. 
        /// For any derived objects, make sure to also call base.Update(gameTime) so the collider is also updated!
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            Coll.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// What should this object the moment of a collision?
        /// For subclasses, make sure to call base.Handle....... so it won't track collisions in the wrong state.
        /// </summary>
        /// <param name="sender">Who sent this collision?</param>
        /// <param name="e"></param>
        protected abstract void HandleCollisionEnter(object sender, EventArgs e);

        /// <summary>
        /// What should this object do the moment a collision ends?
        /// For subclasses, make sure to call base.Handle....... so it won't track collisions in the wrong state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected abstract void HandleCollisionExit(object sender, EventArgs e);


    }
}
