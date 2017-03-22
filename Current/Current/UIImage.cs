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
    /// An Image part of the user interface
    /// </summary>
    class UIImage : UIObject
    {
        public UIImage(string name, Texture2D texture, Point size, Anchor anchor, SortingMode sortingMode, GameState activeState, Point offset, Color color) : base(name, anchor, sortingMode, activeState, offset)
        {
            //Store info
            Texture = texture;
            Location = new Rectangle(Point.Zero, size);
            DrawColor = color;

            //Anchor it
            UIManager.AddAnchoredObject(this, anchor, sortingMode, offset);
        }
    }
}
