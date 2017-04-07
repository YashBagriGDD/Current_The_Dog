using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
    abstract class UIObject : GameObject
    {

        /// <summary>
        /// Called when mouse enters this object
        /// </summary>
        public Action HoverBegin { get; set; }
        /// <summary>
        /// Called when mouse leaves this object
        /// </summary>
        public Action HoverEnd { get; set; }
        /// <summary>
        /// Called when this object is clicked
        /// </summary>
        public Action Click { get; set; }

        /// <summary>
        /// Current Anchor
        /// </summary>
        public Anchor anchor { get; protected set; }
        /// <summary>
        /// Current sorting mode
        /// </summary>
        public SortingMode sortingMode { get; protected set; }

        protected bool cursorWasInRect = false;

        /// <summary>
        /// Construct and draw a new UIObject
        /// </summary>
        /// <param name="name">Name of UIObject</param>
        /// <param name="offset">Offset from calculated anchor position</param>
        /// <param name="anchor">Where is this anchored to on the screen</param>
        /// <param name="sortingMode">How to organize with repsect to other UIObjects</param>
        /// <param name="activeState">GameState when this should be drawn</param>
        public UIObject(string name, Anchor anchor, SortingMode sortingMode, GameState activeState, Point offset) :
            base(name, Rectangle.Empty)
        {
            ActiveState = activeState;
            this.anchor = anchor;
            this.sortingMode = sortingMode;
        }

        public override void Update(GameTime gameTime)
        {

            if (!InTheRightState)
                return;

            //Check if clicked on
            if (InputManager.MouseClickedDown() && Location.Contains(InputManager.MousePos))
                OnClick();
            //Check if Hover began
            if (Location.Contains(InputManager.MousePos) && !cursorWasInRect)
                OnHoverBegin();
            //Check if Hover ended
            if (!Location.Contains(InputManager.MousePos) && cursorWasInRect)
                OnHoverEnd();
            


            cursorWasInRect = Location.Contains(InputManager.MousePos);
            base.Update(gameTime);
        }

        /// <summary>
        /// Event handler for beginning of mouse hover
        /// </summary>
        protected void OnHoverBegin()
        {
            if (HoverBegin != null)
                HoverBegin();
        }

        /// <summary>
        /// Event handler for end of mouse hover
        /// </summary>
        protected void OnHoverEnd()
        {
            if (HoverEnd != null)
                HoverEnd();
        }

        /// <summary>
        /// Event handler for mouse click
        /// </summary>
        protected void OnClick()
        {
            if (Click != null)
                Click();
        }

    }

}
