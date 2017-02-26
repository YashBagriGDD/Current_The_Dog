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
        private Texture2D tex;

        public Collider Coll { get; set; }

        /// <summary>
        /// Make a new Collidable Object and instantiate a collider
        /// </summary>
        /// <param name="texture"></param>
        public CollidableObject(string name, Texture2D texture) : base(name, texture)
        {
            Collider collider = new Collider(this);
            Coll = collider;
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
        }


        /// <summary>
        /// What should this object the moment of a collision?
        /// </summary>
        /// <param name="sender">Who sent this collision?</param>
        /// <param name="e"></param>
        protected abstract void HandleCollisionEnter(object sender, EventArgs e);

        /// <summary>
        /// What should this object the moment a collision ends?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected abstract void HandleCollisionExit(object sender, EventArgs e);


    }
}
