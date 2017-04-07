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
    /// Handles displaying of the score by utilizing UIText and just editing the Update
    /// </summary>
    class Score : UIText
    {
        public Score(string name, SpriteFont font, Anchor anchor, SortingMode sortingMode, GameState activeState, Point offset, Color color) : base(name, "Score: 9999", font, anchor, sortingMode, activeState, offset, color)
        {
        }

        public override void Update(GameTime gameTime)
        {
            Text = "Score: " + GameManager.Score;
            
            base.Update(gameTime);
        }
    }
}
