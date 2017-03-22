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
    /// A button. Quite exciting huh? 
    /// </summary>
    class UIButton : UIObject
    {
        /// <summary>
        /// The text component of the button
        /// </summary>
        public UIText TextComponent { get; protected set; }


        public UIButton(string name, string text, SpriteFont font, Texture2D background, Anchor anchor, SortingMode sortingMode, GameState activeState, Point offset, Color textColor, Color bgColor) : base(name, anchor, sortingMode, activeState, offset)
        {
            //Store info
            Texture = background;
            DrawColor = bgColor;

            //Construct the UIText object
            TextComponent = new UIText("Text_" + name, text, font, anchor, sortingMode, activeState, offset, textColor);

            //Calculate image size and location, based on the text
            Point imgSize = new Point((int)(TextComponent.Location.Width * 1.25f), (int)(TextComponent.Location.Height * 1.25f));

            //Center the background
            Point imgLoc = new Point(TextComponent.Location.X - (imgSize.X - TextComponent.Location.Width)/2, 
                TextComponent.Location.Y - (imgSize.Y - TextComponent.Location.Height)/2);

            //Set Location of the image -- The text is already set by the UIText Constructor
            Location = new Rectangle(imgLoc, imgSize);


            //Set up a hover effect
            float tintFactor = .4f;
            Color shade = new Color((int)(bgColor.R + (255-bgColor.R)*tintFactor), (int)(bgColor.G + (255 - bgColor.G) * tintFactor), (int)(bgColor.B + (255 - bgColor.B) * tintFactor), bgColor.A);

            HoverBegin += () =>
            {
                DrawColor = shade;
            };
            HoverEnd += () =>
            {
                DrawColor = bgColor;
            };

        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (GameManager.gameState != ActiveState)
                return;

            //Only need to draw background, UIText does the text
            spriteBatch.Draw(Texture, Location, DrawColor);
        }
    }
}
