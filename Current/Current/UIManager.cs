using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Current
{

    /// <summary>
    /// Enum representing various anchor points on the display
    /// </summary>
    enum Anchor
    {
        UpperLeft, UpperMiddle, UpperRight,
        CenterLeft, CenterMiddle, CenterRight,
        LowerLeft, LowerMiddle, LowerRight
    }

    /// <summary>
    /// How the elements should be arranged
    /// Ex: SortingMode.Below positions objects below the last positioned object with the same anchor
    /// </summary>
    enum SortingMode
    {
        None,
        Left,
        Right,
        Above,
        Below
    }

    /// <summary>
    /// Static class to manage UI 
    /// </summary>
    static class UIManager
    {
        /// <summary>
        /// Nested class that holds all data to send an instruction to calculate position
        /// </summary>
        private class UIInstruction
        {
            public UIObject uiObject;
            public Anchor anchor;
            public SortingMode sortingMode;
            public Point offset;

            public UIInstruction(UIObject ui, Anchor a, SortingMode s, Point o)
            {
                uiObject = ui;
                anchor = a;
                sortingMode = s;
                offset = o;
            }
        }


        /// <summary>
        /// This is only those that are NOT SortingMode.None
        /// </summary>
        static List<List<UIObject>> AnchoredObjects = new List<List<UIObject>>();

        /// <summary>
        /// A queue of instructions to send to the manager.
        /// </summary>
        static Queue<UIInstruction> Instructions = new Queue<UIInstruction>();

        //Shortcuts for screen width and height
        static int w = Game1.WindowWidth;
        static int h = Game1.WindowHeight;

        /// <summary>
        /// Setup static variables
        /// </summary>
        static UIManager()
        {
            foreach (var anchor in Enum.GetValues(typeof(Anchor)))
            {
                AnchoredObjects.Add(new List<UIObject>());
            }
        }

        /// <summary>
        /// Adds an instruction to the queue to be calucated later. Will NOT actually position a UIObject.
        /// </summary>
        /// <param name="obj">The UIObject to position</param>
        /// <param name="anchor">The anchor type</param>
        /// <param name="sortingMode">The sorting mode</param>
        /// <param name="offset">Offset from calculated position</param>
        public static void AddAnchoredObject(UIObject obj, Anchor anchor, SortingMode sortingMode, Point offset)
        {
            Instructions.Enqueue(new UIInstruction(obj, anchor, sortingMode, offset));
        }

        /// <summary>
        /// Calculate and set UI Object's positions.
        /// This must be called for any ui centering to occur, and the Instructions queue should have been filled first with AddAnchoredObject
        /// </summary>
        public static void OrganizeObjects()
        {
            while (Instructions.Count > 0)
            {
                UIInstruction instr = Instructions.Dequeue();

                SetPosition(instr.uiObject, instr.anchor);
                AdjustPositionBySortingMode(instr.uiObject, (int)instr.anchor, instr.sortingMode);
                //Add offset
                instr.uiObject.Location.X += instr.offset.X;
                instr.uiObject.Location.Y += instr.offset.Y;
                //Add to list to keep track of anchored objects
                if (instr.sortingMode != SortingMode.None)
                    AnchoredObjects[(int)instr.anchor].Add(instr.uiObject);
            }
        }

        /// <summary>
        /// Sets object's position based on provided anchor. [Helper method]
        /// </summary>
        private static void SetPosition(UIObject obj, Anchor anchor)
        {
            switch (anchor)
            {
                case Anchor.UpperLeft:
                    obj.Location.X = 0;
                    obj.Location.Y = 0;
                    break;
                case Anchor.UpperMiddle:
                    obj.Location.X = w / 2 - obj.Location.Width / 2;
                    obj.Location.Y = 0;
                    break;
                case Anchor.UpperRight:
                    obj.Location.X = w - obj.Location.Width;
                    obj.Location.Y = 0;
                    break;
                case Anchor.CenterLeft:
                    obj.Location.X = 0;
                    obj.Location.Y = h / 2 - obj.Location.Height / 2;
                    break;
                case Anchor.CenterMiddle:
                    obj.Location.X = w / 2 - obj.Location.Width / 2;
                    obj.Location.Y = h / 2 - obj.Location.Height / 2;
                    break;
                case Anchor.CenterRight:
                    obj.Location.X = w - obj.Location.Width;
                    obj.Location.Y = h / 2 - obj.Location.Height / 2;
                    break;
                case Anchor.LowerLeft:
                    obj.Location.X = 0;
                    obj.Location.Y = h - obj.Location.Height;
                    break;
                case Anchor.LowerMiddle:
                    obj.Location.X = w / 2 - obj.Location.Width / 2;
                    obj.Location.Y = h - obj.Location.Height;
                    break;
                case Anchor.LowerRight:
                    obj.Location.X = w - obj.Location.Width;
                    obj.Location.Y = h - obj.Location.Height;
                    break;
            }
        }
        /// <summary>
        /// Helper method to slide the UIObject over based on the sorting mode and objects already placed
        /// </summary>
        private static void AdjustPositionBySortingMode(UIObject obj, int anchorIndex, SortingMode sortingMode)
        {
            switch (sortingMode)
            {
                case SortingMode.None:
                    break;
                case SortingMode.Left:
                    foreach (UIObject ui in AnchoredObjects[anchorIndex])
                        if (ui.AreStatesEqual(obj))
                            obj.Location.X -= ui.Location.Width;
                    break;
                case SortingMode.Right:
                    foreach (UIObject ui in AnchoredObjects[anchorIndex])
                        if (ui.AreStatesEqual(obj))
                            obj.Location.X += ui.Location.Width;
                    break;
                case SortingMode.Above:
                    foreach (UIObject ui in AnchoredObjects[anchorIndex])
                        if (ui.AreStatesEqual(obj))
                            obj.Location.Y -= ui.Location.Height;
                    break;
                case SortingMode.Below:
                    foreach (UIObject ui in AnchoredObjects[anchorIndex])
                        if (ui.AreStatesEqual(obj))
                            obj.Location.Y += ui.Location.Height;
                    break;
                default:
                    break;
            }
        }
    }
}
