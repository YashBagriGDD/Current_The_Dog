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

        /// <summary>
        /// Who are we scrolling based on?
        /// </summary>
        public GameObject Target;


        /// <summary>
        /// The background behind us used to create the scrolling illusion.
        /// </summary>
        private ParallaxBackground BehindBackground;


        public ParallaxBackground(string name, Texture2D tex, Rectangle loc, GameState state, int speed, GameObject target) : base(name, tex, loc, state)
        {
            ScrollSpeed = speed;
            Target = target;
            Location = new Rectangle(0, 0, Game1.TargetWidth, Game1.TargetHeight);

        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Active)
            {
                if (Target != null)
                {
                    //Location.X = Target.Location.X;
                }
            }
        }


    }
}
