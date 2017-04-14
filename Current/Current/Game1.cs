using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Current
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        SpriteFont font, titleFont, hudFont;

        //Target Window resolution
        public static int TargetWidth = 1280, TargetHeight = 720;

        //Sizes for level import
        private static int TileWidth = 100, TileHeight = 100;
        private static int ImportWidth = 800, ImportHeight = 480;



        //Debugging variables
        private double fps = 0;

        
        /// <summary>
        /// Actual monitor size as a Point
        /// </summary>
        public Point MonitorSize
        {
            get
            {
                return new Point(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            }
        }

        /// <summary>
        /// Current window size as a point
        /// </summary>
        public Point WindowSize
        {
            get
            {
                return new Point(Window.ClientBounds.Width, Window.ClientBounds.Height);
            }
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Set window size
            graphics.PreferredBackBufferWidth = TargetWidth;
            graphics.PreferredBackBufferHeight = TargetHeight;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            InputManager.Init();

            IsMouseVisible = true;
            base.Initialize();
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load textures
            Texture2D texCurSwim = Content.Load<Texture2D>("Textures/Current/CurrentSwim");
            Texture2D texBlock = Content.Load<Texture2D>("Textures/WhiteBlock");
            Texture2D texBG1 = Content.Load<Texture2D>("Textures/Backgrounds/Background");
            Texture2D texHealth = Content.Load<Texture2D>("Textures/HUD/Crossbone");

            //Load fonts
            font = Content.Load<SpriteFont>("Fonts/Font");
            titleFont = Content.Load<SpriteFont>("Fonts/TitleFont");
            hudFont = Content.Load<SpriteFont>("Fonts/HudFont");

            //Parse the level file
            List<SaveTile> tiles = GameManager.ParseLevel("Text/level.txt");

            float xRatio = (float)TargetWidth / ImportWidth;
            float YRatio = (float)TargetHeight / ImportHeight;



            //Load in the Background
            Background b = new Background("bg1", texBG1, new Rectangle(0, 0, TargetWidth, TargetHeight));

            //Load in the level tiles
            int numWater = 0, numPlatforms = 0;
            foreach (SaveTile tile in tiles)
            {
                string type = GameManager.TextureMapping[tile.TextureName];
                Texture2D tex = Content.Load<Texture2D>("textures/tiles/" + tile.TextureName);                                      //Some height lost here, so multiply it 
                Rectangle loc = new Rectangle((int)(tile.X * xRatio), (int)(tile.Y * YRatio), (int)(TileWidth * xRatio), (int)(TileHeight * YRatio * 1.2f));
                switch (type)
                {
                    case "Water":
                        Water w = new Water("Water" + numWater, tex, loc, Vector2.Zero);
                        numWater++;
                        break;
                    case "Platform":
                        Platform p = new Platform("Platform" + numPlatforms, tex, loc);
                        numPlatforms++;
                        break;
                }
            }

            

            //Setup more complicated objects
            Player player = new Player("Current", texCurSwim, new Rectangle(100, 0, 100, 100));
            Animate playerSwim = new Animate(texCurSwim, 3, 3, Animate.ONESIXTIETHSECPERFRAME * 10, player);
            player.AddAnimation(playerSwim);


            Color buttonBackColor = new Color(50, 80, 130);
            int bWidth = 400, bHeight = 100;


            //Create the Main Menu
            Background bMainMenu = new Background("bgMainMenu", texBG1, new Rectangle(0, 0, TargetWidth, TargetHeight));
            bMainMenu.ActiveState = GameState.MainMenu;

            //Main Menu Substate
            UIText title = new UIText("Title", "Current", titleFont, Anchor.UpperMiddle, SortingMode.None, GameState.MainMenu, Point.Zero, Color.White);

            UIButton play = new UIButton("PlayB", "Play", font, texBlock, Anchor.CenterMiddle, SortingMode.Below, GameState.MainMenu, Point.Zero, Color.White, buttonBackColor, bWidth, bHeight);
            UIButton quit = new UIButton("QuitB", "Quit", font, texBlock, Anchor.CenterMiddle, SortingMode.Below, GameState.MainMenu, new Point(0, 50), Color.White, buttonBackColor, bWidth, bHeight);

            //Setup button delegates
            play.Click += () =>
            {
                GameManager.gameState = GameState.Game;         
            };

            quit.Click += () =>
            {
                Exit();
            };

            //Setup HUD
            HealthBar bar = new HealthBar("HealthBar", texHealth, new Point(100, 66));
            Score score = new Score("Score", hudFont, Anchor.UpperRight, SortingMode.None, GameState.Game, Point.Zero, Color.White);



            //Add pickups
            ScorePickup scorePickup = new ScorePickup("ScorePickup1", texBlock, new Rectangle(500, 100, 100, 100), 10);
            HealthPickup healthPickup = new HealthPickup("HealthPickup1", texBlock, new Rectangle(200, 100, 100, 100), 1);
            healthPickup.DrawColor = Color.Red;


            //Setup the pause menu
            UIText pauseText = new UIText("pauseText", "PAUSED", font, Anchor.UpperMiddle, SortingMode.Below, GameState.Game, Point.Zero, Color.White);
            pauseText.ActiveGameplayState = GameplayState.Paused;
            UIButton pauseResumeButton = new UIButton("pauseResumeButton", "Resume", font, texBlock, Anchor.CenterMiddle, SortingMode.Below, GameState.Game, new Point(0, 0), Color.White, buttonBackColor);
            pauseResumeButton.ActiveGameplayState = GameplayState.Paused;
            UIButton pauseMainMenuButton = new UIButton("pauseMainMenuButton", "Main Menu", font, texBlock, Anchor.CenterMiddle, SortingMode.Below, GameState.Game, new Point(0, 50), Color.White, buttonBackColor);
            pauseMainMenuButton.ActiveGameplayState = GameplayState.Paused;



            ////Pause menu delegates
            pauseResumeButton.Click += () =>
            {
                GameManager.gameplayState = GameplayState.Normal;
            };


            pauseMainMenuButton.Click += () =>
            {
                GameManager.gameplayState = GameplayState.Normal;
                GameManager.gameState = GameState.MainMenu;
                GameManager.mainMenuState = MainMenuState.MainMenu;
                GameManager.ResetAll();
            };

            UIManager.OrganizeObjects();


        }

        /// <summary>
        /// Toggles between fullscreen and windowed modes
        /// </summary>
        public void ToggleFullscreen()
        {
            graphics.IsFullScreen = !graphics.IsFullScreen;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            //Update all the objects
            foreach (GameObject g in GameManager.GetAll().Values)
            {
                g.Update(gameTime);
            }
            //GameManager requires its own update
            GameManager.Update(gameTime);


            //InputManager requires its own Update -- Make sure this is the last update call
            InputManager.Update(gameTime);


            fps = 1.0f / gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);


            spriteBatch.Begin();

            //Draw all the objects
            foreach (GameObject g in GameManager.GetAll().Values)
            {
                g.Draw(gameTime, spriteBatch);
            }
            //Uncomment line below to show FPS
            //spriteBatch.DrawString(font, fps.ToString(), new Vector2(0, 0), Color.White);

            /*Wanna see some states? Uncomment!
            spriteBatch.DrawString(font, GameManager.gameState.ToString(), new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(font, GameManager.gameplayState.ToString(), new Vector2(0, 50), Color.White);
            spriteBatch.DrawString(font, GameManager.mainMenuState.ToString(), new Vector2(0, 100), Color.White);*/



            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
