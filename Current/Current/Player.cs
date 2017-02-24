using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Current
{
    class Player : CollidableObject
    {

        public int Health { get; set; }
        public Vector2 JumpForce { get; set; }
        public bool OnLand { get; set; }
        public float SpeedInWater { get; set; }
        public float SpeedOnLand { get; set; }
        public int Strength { get; set; }


        public Player(string name, Texture2D tex) : base(name, tex)
        {
            throw new NotImplementedException();
        }
        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        protected override void HandleCollision(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Die()
        {
            throw new NotImplementedException();
        }
        public void Hurt()
        {
            throw new NotImplementedException();
        }

        public void Jump()
        {
            throw new NotImplementedException();
        }

        public void MoveInWater()
        {
            throw new NotImplementedException();
        }

        public void MoveOnLand()
        {
            throw new NotImplementedException();
        }
    }
}
