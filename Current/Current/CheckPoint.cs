using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Current
{
    class CheckPoint : Pickup
    {


        /// <summary>
        /// Has the checkpoint been passed by the player?
        /// </summary>
        public bool Passed { get; set; }


        /// <summary>
        /// Makes a checkpoint.
        /// </summary>
        /// <param name="tex"></param>
        public CheckPoint(string name, Texture2D tex, Rectangle location) : base(name, tex, location, 0)
        {
            Passed = false;
            //Add 
            AddAnimation(new Animate(Game1.Textures["Checkpoint"], 1, 1, 1, this));
            AddAnimation(new Animate(Game1.Textures["CheckpointHit"], 1, 1, 1, this));
            ChangeAnimation("Checkpoint");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);   
        }

        protected override void HandleCollisionEnter(object sender, EventArgs e)
        {
            //Include sfx logic
            if (!Passed)
                base.HandleCollisionEnter(sender, e);

            Collider other = (Collider)sender;
            //Only call this if we haven't been passed yet
            if (other.Host is Player && !Passed)
            {
                Player player = (Player)(other.Host);
                //Update player's start location
                player.SpawnLocation = new Rectangle(Location.X, Location.Y, player.Location.Width, player.Location.Height);
                //Don't let this checkpoint be triggered again
                Passed = true;

                ChangeAnimation("CheckpointHit");

            }
        }

        protected override void HandleCollisionExit(object sender, EventArgs e)
        {
        }
    }
}
