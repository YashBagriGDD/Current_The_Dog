using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Current
{
    class Collider : GameObject
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
        /// The Collision EventHandler.
        /// Call OnCollision when there is a collision, and the event will fire!
        /// </summary>
        public event EventHandler Collision;


        public Collider(string name, GameObject host) : base(name)
        {
            Host = host;
            Hitbox = new Rectangle((int)Host.Position.X, (int)Host.Position.Y, Host.Texture.Width, Host.Texture.Height);

        }

        /// <summary>
        /// Calls the Collision event
        /// </summary>
        public void OnCollision(Collider source)
        {
            if (Collision != null)
            {
                Collision(source, EventArgs.Empty);
                
            }
        }

        public override void Update(GameTime gameTime)
        {
            Hitbox = new Rectangle((int)Host.Position.X, (int)Host.Position.Y, Host.Texture.Width, Host.Texture.Height);
            foreach (CollidableObject c in GameManager.CollidableObjects)
            {
                if (c.Name == Host.Name)
                    continue;
                if (Hitbox.Intersects(c.Coll.Hitbox))
                {
                    OnCollision(c.Coll);
                }
            }
            

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
           
        }
    }
}
