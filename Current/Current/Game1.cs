using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Current
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        SpriteFont font, titleFont;

        //Window sizes
        public static int WindowWidth = 1280, WindowHeight = 720;

        //Sizes for level import
        private static int TileWidth = 100, TileHeight = 100;
        private static int ImportWidth = 800, ImportHeight = 480;

        //Debugging variables
        private double fps = 0;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Set window size
            graphics.PreferredBackBufferWidth = WindowWidth;
            graphics.PreferredBackBufferHeight = WindowHeight;
            graphics.ApplyChanges();
            //Uncomment line below for fullscreen
            //graphics.IsFullScreen = true;
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

            //Load fonts
            font = Content.Load<SpriteFont>("Fonts/Font");
            titleFont = Content.Load<SpriteFont>("Fonts/TitleFont");

            //Parse the level file
            List<SaveTile> tiles = GameManager.ParseLevel("Text/level.txt");

            float xRatio = (float)WindowWidth / ImportWidth;
            float YRatio = (float)WindowHeight/ ImportHeight;



            //Load in the Background
            Background b = new Background("bg1", texBG1, new Rectangle(0, 0, WindowWidth, WindowWidth));

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
            Animate playerSwim = new Animate(texCurSwim, 3, 3, Animate.ONESIXTIETHSECPERFRAME*10, player);
            player.AddAnimation(playerSwim);


            Color buttonBackColor = new Color(50, 80, 130);
            int bWidth = 400, bHeight = 100;


            //Create the Main Menu
            Background bMainMenu = new Background("bgMainMenu", texBG1, new Rectangle(0, 0, WindowWidth, WindowWidth));
            bMainMenu.ActiveState = GameState.MainMenu;

            //Main Menu Substate
            UIText title = new UIText("Title", "Current", titleFont, Anchor.UpperMiddle, SortingMode.None, GameState.MainMenu, Point.Zero, Color.White);
            UIButton play = new UIButton("PlayB", "Play", font, texBlock, Anchor.CenterMiddle, SortingMode.Below, GameState.MainMenu, Point.Zero, Color.White, buttonBackColor, bWidth, bHeight);
            UIButton options = new UIButton("OptionsB", "Options", font, texBlock, Anchor.CenterMiddle, SortingMode.Below, GameState.MainMenu, new Point(0,50), Color.White, buttonBackColor, bWidth, bHeight);
            UIButton quit = new UIButton("QuitB", "Quit", font, texBlock, Anchor.CenterMiddle, SortingMode.Below, GameState.MainMenu, new Point(0, 100), Color.White, buttonBackColor, bWidth, bHeight);

            //Setup delegates
            play.Click += () =>
            {
                GameManager.gameState = GameState.Game;
            };

            options.Click += () =>
            {
                GameManager.mainMenuState = MainMenuState.Options;
            };

            quit.Click += () =>
            {
                Exit();
            };



            //Options substate
            UIText titleOptions = new UIText("Options", "Options", titleFont, Anchor.UpperMiddle, SortingMode.None, GameState.MainMenu, Point.Zero, Color.White);
                titleOptions.ActiveMainMenuState = MainMenuState.Options;
           // UIButton play = new UIButton("PlayB", "Play", font, texBlock, Anchor.CenterMiddle, SortingMode.Below, GameState.MainMenu, Point.Zero, Color.White, buttonBackColor, bWidth, bHeight);
           // UIButton options = new UIButton("OptionsB", "Options", font, texBlock, Anchor.CenterMiddle, SortingMode.Below, GameState.MainMenu, new Point(0, 50), Color.White, buttonBackColor, bWidth, bHeight);
           // UIButton quit = new UIButton("QuitB", "Quit", font, texBlock, Anchor.CenterMiddle, SortingMode.Below, GameState.MainMenu, new Point(0, 100), Color.White, buttonBackColor, bWidth, bHeight);



            /*Example UIText Objects.
            UIText t = new UIText("demoFont", "Test", font, Anchor.UpperLeft, SortingMode.Below, GameState.Game, Point.Zero, Color.Red);
            UIText t2 = new UIText("demoFont2", "Test", font, Anchor.UpperRight, SortingMode.Below, GameState.Game, Point.Zero, Color.White);
            UIText t3 = new UIText("demoFont3", "Test", font, Anchor.UpperMiddle, SortingMode.Below, GameState.Game, Point.Zero, Color.Black);

            //This is how you set up actions for UI Objects
            t.HoverBegin += () =>
            {
                t.Text = "Dogs";
            };*/

            /* UIButton button = new UIButton("button", "Menu", font, texBlock, Anchor.UpperLeft, SortingMode.Below, GameState.Game, Point.Zero, Color.White, Color.Black);

             button.Click += () =>{
                 GameManager.gameState = GameState.MainMenu;
             };

             UIText text = new UIText("menu", "There's nothing here right now...Check back later", font, Anchor.CenterMiddle, SortingMode.Below, GameState.MainMenu, Point.Zero, Color.White);*/

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

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Update all the objects
            foreach (GameObject g in GameManager.GetAll().Values)
            {
                g.Update(gameTime);
            }
            //InputManager requires its own Update
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
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
