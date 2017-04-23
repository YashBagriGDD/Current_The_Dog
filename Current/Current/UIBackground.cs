using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Current
{
    /// <summary>
    /// Basically a Background object, but under the UIObject. 
    /// Allows for diversification between main menu and game backgrounds for easier loading.
    /// </summary>
    class UIBackground: UIObject
    {
        public UIBackground(string name, Texture2D tex, GameState state) : base(name, Anchor.UpperLeft, SortingMode.None, state, Point.Zero)
        {
            ActiveState = state;
            //Stretch across entire screen.
            Location = new Rectangle(0, 0, Game1.TargetWidth, Game1.TargetHeight);
        }
    }
}
