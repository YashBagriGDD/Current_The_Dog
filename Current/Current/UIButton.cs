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


        /// Override these properties to make state changes active in the text as well as the button
        private GameplayState activeGameplayState = GameplayState.Normal;
        public override GameplayState ActiveGameplayState
        {
            get
            {
                return activeGameplayState;
            }
            set
            {
                activeGameplayState = value;
                if (TextComponent != null)
                    TextComponent.ActiveGameplayState = value;
            }
        }

        private GameState activeGameState = GameState.Game;
        public override GameState ActiveState
        {
            get
            {
                return activeGameState;
            }
            set
            {
                activeGameState = value;
                if (TextComponent != null)
                    TextComponent.ActiveState = value;
            }
        }

        private MainMenuState activeMainMenuState = MainMenuState.MainMenu;
        public override MainMenuState ActiveMainMenuState
        {
            get
            {
                return activeMainMenuState;
            }
            set
            {
                activeMainMenuState = value;
                if (TextComponent != null)
                    TextComponent.ActiveMainMenuState = value;
            }
        }

        public UIButton(string name, string text, SpriteFont font, Texture2D background, Anchor anchor, SortingMode sortingMode, GameState activeState, Point offset, Color textColor, Color bgColor, int fixedWidth=0, int fixedHeight=0) : base(name, anchor, sortingMode, activeState, offset)
        {
            //Store info
            Texture = background;
            DrawColor = bgColor;

            //Construct the UIText object
            TextComponent = new UIText("Text_" + name, text, font, anchor, sortingMode, activeState, offset, textColor);
            TextComponent.Parent = this;
            //Calculate image size and location, based on the text, so long as optional width and heights aren't provided
            Point imgSize;
            if (fixedWidth == 0 || fixedHeight == 0)
                imgSize = new Point((int)(TextComponent.Location.Width * 1.25f), (int)(TextComponent.Location.Height * 1.25f));
            else
                imgSize = new Point(fixedWidth, fixedHeight);

            //Center the background
            Point imgLoc = new Point(TextComponent.Location.X - (imgSize.X - TextComponent.Location.Width)/2, 
                TextComponent.Location.Y - (imgSize.Y - TextComponent.Location.Height)/2);

            //Set Location of the image -- The text is already set by the UIText Constructor
            Location = new Rectangle(imgLoc, imgSize);


            //Set up a hover effect
            float tintFactor = .9f;
            Color shade = new Color((int)(bgColor.R*tintFactor), (int)(bgColor.G * tintFactor), (int)(bgColor.B * tintFactor), bgColor.A);

            HoverBegin += () =>
            {
                DrawColor = shade;
                GameManager.PlaySFX("Hover");
            };
            HoverEnd += () =>
            {
                DrawColor = bgColor;
            };
            Click += () =>
            {
                GameManager.PlaySFX("Select");
            };

        }

        /// <summary>
        /// Readjust the location--necessary because UIObjects aren't sorted until the end of LoadContent,
        /// so the UIButton needs to be reset since UIManager doensn't directly set it's location - it gets 
        /// it from it's text component.
        /// </summary>
        public void AdjustLocation()
        {
            //Center the background
            Point imgLoc = new Point(TextComponent.Location.X - (Location.Width - TextComponent.Location.Width) / 2,
                TextComponent.Location.Y - (Location.Height - TextComponent.Location.Height) / 2);

            //Set Location of the image -- The text is already set by the UIText Constructor
            Location = new Rectangle(imgLoc.X, imgLoc.Y, Location.Width, Location.Height);
        }



        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Active)
                return;

            //Only need to draw background, UIText does the text
            base.Draw(gameTime, spriteBatch);
        }
    }
}
