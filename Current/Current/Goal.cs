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
    /// Represents the goal of the level
    /// </summary>
    class Goal : Pickup
    {
        public Goal(string name, Texture2D tex, Rectangle location) : base(name, tex, location, 0)
        {

        }


        protected override void HandleCollisionEnter(object sender, EventArgs e)
        {
            Collider other = (Collider)sender;
            if (other.Host is Player)
            {
                //Set player to win state, and freeze them.
                Player p = (Player)(other.Host);
                p.state = PlayerState.HasWon;
                p.Acceleration = Vector2.Zero;
                p.Velocity = Vector2.Zero;
                //Increment level
                GameManager.CurrentLevel++;


                //Turn on the main menu button no matter what
                GameManager.Get("WinMainMenuButton").Activate();


                //Play a sound effect
                GameManager.PlaySFX("Win");

                //We still have more levels to get through, so show the win level text and next level button
                if (!GameManager.CompletedAllLevels)
                {
                    GameManager.Get("WinText").Activate();
                    GameManager.Get("WinNextButton").Activate();
                }
                //No more levels - only additional thing to show is win game text
                else
                {
                    GameManager.Get("WinGameText").Activate();
                }

                //Stop everyone from updating.
                GameManager.StopNonUIUpdates();
            }
        }

        protected override void HandleCollisionExit(object sender, EventArgs e)
        {
        }
    }
}
