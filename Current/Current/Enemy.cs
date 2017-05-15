using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Current
{
    abstract class Enemy : ComplexCollidableObject
    {
        public int Health { get; set; }
        public float Speed { get; set; }
        public int Strength { get; set; } = 10;
        public Vector2 HomePoint { get; set; }

        protected bool collActive; //For disabling and enabling collisions in certain states


        public Enemy(string name, Texture2D tex, Rectangle location) : base(name, tex, location)
        {
        }

        /// <summary>
        /// Respawns and reactivates the enemy
        /// </summary>
        public override void Respawn()
        {
            base.Respawn();
            Activate();
        }
    }
}
