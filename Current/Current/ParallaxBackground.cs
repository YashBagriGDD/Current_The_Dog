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
    /// Child of the Background class that also does parallax scrolling relative to a target
    /// </summary>
    class ParallaxBackground : Background
    {

        /// <summary>
        /// Rate at which we scroll in a parallax fashion.
        /// </summary>
        public int ScrollSpeed { get; set; }


        private GameObject target;
        /// <summary>
        /// Who are we scrolling based on?
        /// </summary>
        public GameObject Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
                if (BehindBackground != null)
                    BehindBackground.target = value;
            }
        }

        /// <summary>
        /// The background behind us used to create the scrolling illusion.
        /// </summary>
        private ParallaxBackground BehindBackground;

        /// <summary>
        /// Offset to make sure background loops back to right spot
        /// </summary>
        private int xOffset = 0;
        /// <summary>
        /// Min and max X positions of BG
        /// </summary>
        private int minX, maxX; 


        /// <summary>
        /// Create a parallax background
        /// </summary>
        /// <param name="speed">Speed at which background scrolls</param>
        /// <param name="target">Who are we focued on?</param>
        /// <param name="isBacking">Is this is the second ParallaxBackground to help create the illusion of the scolling backgound?</param>
        public ParallaxBackground(string name, Texture2D tex, Rectangle loc, GameState state, int speed, GameObject viewTarget, bool isBacking=false) : base(name, tex, loc, state)
        {
            ScrollSpeed = speed;
            Target = viewTarget;
            minX = -Location.Width;
            maxX = Location.Width;
            if (!isBacking)
            {
                //Make a copy that will be offset 
                Rectangle backLoc = new Rectangle(loc.X + loc.Width, loc.Y, loc.Width, loc.Height);
                BehindBackground = new ParallaxBackground(name + "_Backing", tex, backLoc, state, speed, target, true);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Active)
            {
                if (Target != null)
                {
                    Location.X = -Target.Location.X + xOffset + LoadLocation.X;

                    if (Location.X < minX)
                        xOffset += 2*Location.Width;
                    if (Location.X > maxX)
                        xOffset -= 2*Location.Width;

                }
            }
        }
    }
}
