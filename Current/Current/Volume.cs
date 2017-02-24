using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Current
{
    abstract class Volume : CollidableObject
    {
        public Vector2 Force { get; set; }

        public Volume(string name, Texture2D tex, Vector2 force) : base(name, tex)
        {
            throw new NotImplementedException();
        }
    }
}
