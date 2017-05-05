using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Current
{
    class HighscoreDisplay : UIText
    {
        public HighscoreDisplay(string name, SpriteFont font, Anchor anchor, SortingMode sortingMode, GameState activeState, Point offset, Color color) : base(name, "Highscore : 9999", font, anchor, sortingMode, activeState, offset, color)
        {
        }


        public override void Update(GameTime gameTime)
        {
            Text = "Highscore : " + GameManager.HighScore;

            base.Update(gameTime);
        }
    }
}
