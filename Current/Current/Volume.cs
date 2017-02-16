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

        public Volume(Texture2D tex, Vector2 force) : base(tex)
        {
            throw new NotImplementedException();
        }
    }
}
