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
    /// Displays level number. (Simple extension of UIText)
    /// </summary>
    class LevelDisplay : UIText
    {
        public LevelDisplay(string name, SpriteFont font, Anchor anchor, SortingMode sortingMode, GameState activeState, Point offset, Color color) : base(name, "Level 0000", font, anchor, sortingMode, activeState, offset, color)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Text = "Level: " + (GameManager.CurrentLevel + 1);
        }
    }
}
