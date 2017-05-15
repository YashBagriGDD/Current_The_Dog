using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Current
{
    /// <summary>
    /// Collider class to be attached to GameObjects.
    /// Broadcasts messages for OnCollisionEnter, exit, and allows for checking of collisions each frame.
    /// </summary>
    class Collider
    {
        /// <summary>
        /// GameObject this is attached to
        /// </summary>
        public CollidableObject Host { get; set; }

        public float Gravity { get; set; }

        public bool Solid { get; set; }

        //The actual rectangle for collisions
        public Rectangle Hitbox;
        //The rectangle containing offset values
        private Rectangle hitboxOffset;
        //Should this collider broadcast events?
        private bool sendEvents;

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
        /// <summary>
        /// Should this collider actually do the checking of collisions?
        /// Included for performance optimization.
        /// </summary>
        public bool ChecksCollisions { get; set; } = true;


        /// <summary>
        /// Create a standard collider attached to the host GameObject
        /// </summary>
        public Collider(CollidableObject host) 
        {
            Host = host;
            CurrentCollisions = new HashSet<Collider>();
            Hitbox = new Rectangle(Host.Location.X, Host.Location.Y, Host.Location.Width, Host.Location.Height);
            hitboxOffset = new Rectangle(0, 0, 1, 1); // no offset
            sendEvents = true;
        }


        /// <summary>
        /// Create a collider offset from the host.
        /// This will not broadcast events to avoid redundant events being sent out
        /// </summary>
        /// <param name="host">The Host GameObject</param>
        /// <param name="offset">A rectangle representing the x-offset, y-offset, width, height </param>
        public Collider(CollidableObject host, Rectangle offset)
        {
            Host = host;
            CurrentCollisions = new HashSet<Collider>();

            //Apply the offset
            hitboxOffset = offset;
            Hitbox = new Rectangle(Host.Location.X + offset.X, Host.Location.Y + offset.Y,
                offset.Width, offset.Height);
            sendEvents = false;

        }

        /// <summary>
        /// Resets the Hitbox to the Host's position plus an offset
        /// Note: the offset x and y can be zero.
        /// </summary>
        private void UpdateHitbox()
        {
            Hitbox.X = Host.Location.X + hitboxOffset.X;
            Hitbox.Y = Host.Location.Y + hitboxOffset.Y;
        }

        /// <summary>
        /// Calls the Collision event
        /// </summary>
        /// <param name="source">Who am I colliding with?</param>
        public void OnCollision(Collider source)
        {
            if (!sendEvents || !source.Host.Active) 
                return;
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
            if (!sendEvents || !source.Host.Active)
                return;
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
            if (!other.Active)
                return false;
            return CurrentCollisions.Contains(other.Coll);
        }

        /// <summary>
        /// Am I colliding with type T of object? 
        /// </summary>
        /// <typeparam name="T">The Type to check</typeparam>
        public bool CollidingWith<T>()
        {
            foreach (Collider c in CurrentCollisions)
                if (c.Host is T && c.Host.Active)
                    return true;
            return false;
        }

        /// <summary>
        /// Am I colliding with anything?
        /// </summary>
        public bool CollidingWithAnything()
        {
            foreach (Collider c in CurrentCollisions)
                if (c.Host.Active)
                    return true;
            return false;
        }

        /// <summary>
        /// Clear everything that we're colliding with 
        /// </summary>
        public void ResetCollider()
        {
            CurrentCollisions.Clear();

        }

        /// <summary>
        /// Called every frame by the CollidableObject, which is called every frame by the Game1 class.
        /// Updates a Rectangle at the location of the Host GameObject each frame. 
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //Don't bother checking collisions if not active
            if (!Host.Active)
                return;

            UpdateHitbox();

            if (!ChecksCollisions)
                return;

            foreach (CollidableObject c in GameManager.CollidableObjectsInView)
            {
                if (!c.Active)
                    continue;
                //Don't check collisions with yourself!
                if (c.Name == Host.Name)
                    continue;
                //If we intersect with a new object
                if (Hitbox.Intersects(c.Coll.Hitbox) && !CurrentCollisions.Contains(c.Coll))
                {
                    CurrentCollisions.Add(c.Coll);
                    OnCollision(c.Coll);
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
