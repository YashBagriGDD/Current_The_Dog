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
    /// 
    /// </summary>
    class Background : UIObject
    {
        public Background(string name, Texture2D tex, Rectangle loc, GameState state) : base(name, Anchor.UpperLeft, SortingMode.None, state, Point.Zero)
        {
            Location = loc;
            Texture = tex;
        } 
    }
}
