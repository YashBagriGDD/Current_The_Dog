using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

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

        public Rectangle Hitbox { get; set; }

        /// <summary>
        /// Type of Collider. 
        /// This should probably its own data type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The CollisionEnter event.
        /// Fires the moment a collision occurs
        /// </summary>
        public event EventHandler CollisionEnter;

        /// <summary>
        /// The CollisionExit event.
        /// Fires the moment a collision stops occurring 
        /// </summary>
        public event EventHandler CollisionExit;

        /// <summary>
        /// A set of the current collisions
        /// </summary>
        public HashSet<Collider> CurrentCollisions { get; private set; }



        public Collider(GameObject host) 
        {
            Host = host;
            CurrentCollisions = new HashSet<Collider>();
            UpdateHitbox();
        }

        /// <summary>
        /// Resets the Hitbox to the Host's position
        /// </summary>
        private void UpdateHitbox()
        {
            Hitbox = new Rectangle((int)Host.Position.X, (int)Host.Position.Y, (int)(Host.Texture.Width * Host.Scale.X), (int)(Host.Texture.Height * Host.Scale.Y));
        }

        /// <summary>
        /// Calls the Collision event
        /// </summary>
        /// <param name="source">Who am I colliding with?</param>
        public void OnCollision(Collider source)
        {
            if (CollisionEnter != null)
            {
                CollisionEnter(source, EventArgs.Empty);
                
            }
        }

        /// <summary>
        /// Calls the CollisionExit event
        /// </summary>
        /// <param name="source">Who was I colliding with?</param>
        public void OnCollisionExit(Collider source)
        {
            if (CollisionExit != null)
            {
                CollisionExit(source, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Am I currently colliding with the other CollidableObject? 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool CollidingWith(CollidableObject other)
        {
            return CurrentCollisions.Contains(other.Coll);
        }

        /// <summary>
        /// Am I colliding with anything?
        /// </summary>
        /// <returns></returns>
        public bool CollidingWithAnything()
        {
            return (CurrentCollisions.Count > 0);
        }

        /// <summary>
        /// Called every frame by the CollidableObject, which is called every frame the Game1 class.
        /// Creates a Rectangle at the location of the Host GameObject each frame. 
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            UpdateHitbox();
            foreach (CollidableObject c in GameManager.CollidableObjects)
            {
                //Don't check collisions with yourself!
                if (c.Name == Host.Name)
                    continue;
                //If we intersect with a new object
                if (Hitbox.Intersects(c.Coll.Hitbox) && !CurrentCollisions.Contains(c.Coll))
                {
                    OnCollision(c.Coll);
                    CurrentCollisions.Add(c.Coll);
                }
                //If stop intersecting with an object we just were intersecting with
                if (!Hitbox.Intersects(c.Coll.Hitbox) && CurrentCollisions.Contains(c.Coll))
                {
                    OnCollisionExit(c.Coll);
                    CurrentCollisions.Remove(c.Coll);
                }
            }
            

        }



        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
           
        }
    }
}
