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
    /// A background element.
    /// </summary>
    class Background : GameObject
    {
        public Background(string name, Texture2D tex, Rectangle loc, GameState state) : base(name, tex, loc)
        {
            ActiveState = state;
            AlwaysDraw = true;
        }
    }
}
