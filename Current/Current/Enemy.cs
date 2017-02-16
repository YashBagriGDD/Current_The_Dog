using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Current
{
    abstract class Enemy : CollidableObject
    {
        public int Health { get; set; }
        public float Speed { get; set; }
        public int Strength { get; set; }

        public Enemy(Texture2D tex) : base(tex)
        {
            throw new NotImplementedException();
        }
    }
}
