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
    /// The player's laser projectile!
    /// </summary>
    class Laser : CollidableObject
    {

        /// <summary>
        /// The object this is attached to 
        /// </summary>
        GameObject Parent;

        /// <summary>
        /// Offset between parent and laser position
        /// </summary>
        public Point Offset;

        public Laser(string name, Texture2D texture, Rectangle location, GameObject Parent, Point offset) : base(name, texture, location)
        {
            this.Parent = Parent;
            Offset = offset;
            Deactivate();

            Rotation = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Snap to parent location
            if (Active)
            {
                Location.X = Parent.Location.X + Offset.X;
                Location.Y = Parent.Location.Y + Offset.Y;

            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }


        protected override void HandleCollisionEnter(object sender, EventArgs e)
        {
            Collider other = (Collider)sender;
            if (other.Host is Enemy)
            {
                Enemy enemy = (Enemy)(other.Host);
                if (enemy.Active)
                    enemy.Deactivate();
                GameManager.Score += enemy.Strength;
            }
        }

        protected override void HandleCollisionExit(object sender, EventArgs e)
        {
        }
    }
}
