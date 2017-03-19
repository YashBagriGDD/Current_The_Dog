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
    /// Represents an object for the user interface, whether that be the HUD or menu
    /// </summary>
    abstract class UIObject: GameObject
    {
        /// <summary>
        /// GameObjects are almost always only drawn in GameState.Game, with the exception of UIObjects
        /// UIObjects will vary with regard to when they're drawn, so an argument is available in the constructor for that purpose.
        /// </summary>
        public UIObject(string name, GameState activeState) : base(name, new Rectangle())
        {
            ActiveState = activeState;
        }

    }
}
