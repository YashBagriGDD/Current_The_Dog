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
                Player p = (Player)(other.Host);
                p.state = PlayerState.HasWon;
                p.Acceleration = Vector2.Zero;
                p.Velocity = Vector2.Zero;
                GameManager.CurrentLevel++;

                GameManager.Get("WinMainMenuButton").Activate();


                //We still have more levels to get through.
                if (!GameManager.CompletedAllLevels)
                {
                    GameManager.Get("WinText").Activate();
                    GameManager.Get("WinNextButton").Activate();
                }
                else
                {
                    GameManager.Get("WinGameText").Activate();
                }


                GameManager.StopNonUIUpdates();
            }
        }

        protected override void HandleCollisionExit(object sender, EventArgs e)
        {
        }
    }
}
