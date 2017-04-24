using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Current
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatchGameplay;
        SpriteBatch spriteBatchUI;
        SpriteBatch spriteBatchBG;


        SpriteFont font, titleFont, hudFont;

        //Target Window resolution
        public static int TargetWidth = 1280, TargetHeight = 720;

        /// <summary>
        /// The main camera
        /// </summary>
        public Camera MainCamera;

        //Sizes for level import
        private static int TileWidth = 100, TileHeight = 100;
        private static int ImportWidth = 800, ImportHeight = 480;
        //Calculate x and y ratios to scale tiles based on new and old resolutions
        float xRatio = (float)TargetWidth / ImportWidth;
        float YRatio = (float)TargetHeight / ImportHeight;


        //Debugging variables
        private double fps = 0;


        //For threading.
        public static object locker = new object();

        
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
                try
                {
                    return new Point(Window.ClientBounds.Width, Window.ClientBounds.Height);
                }
                catch(Exception e)
                {
                    return Point.Zero;

                }
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


        #region Textures
        Texture2D texCurSwim;
        Texture2D texBlock;
        Texture2D texBG1;
        Texture2D texHealth;
        Texture2D texCatfish;
        #endregion

        #region Audio
        Song songBg;
        SoundEffect sfxHover, sfxHurt, sfxJump, sfxLand, sfxPickup, sfxSelect, sfxWaterLoop, sfxWin;
        #endregion

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatchGameplay = new SpriteBatch(GraphicsDevice);
            spriteBatchUI = new SpriteBatch(GraphicsDevice);
            spriteBatchBG = new SpriteBatch(GraphicsDevice);

            //Load textures
            texCurSwim = Content.Load<Texture2D>("Textures/Current/CurrentSwim");
            texBlock = Content.Load<Texture2D>("Textures/WhiteBlock");
            texBG1 = Content.Load<Texture2D>("Textures/Backgrounds/Background");
            texHealth = Content.Load<Texture2D>("Textures/HUD/Crossbone");
            texCatfish = Content.Load<Texture2D>("Textures/Enemies/catfish");

            //Load Background Music
            songBg = Content.Load<Song>("Audio/Current");
            //Load Sound Effects
            sfxHover = Content.Load<SoundEffect>("Audio/Hover");
            sfxHurt = Content.Load<SoundEffect>("Audio/Hurt");
            sfxJump = Content.Load<SoundEffect>("Audio/Jump");
            sfxLand = Content.Load<SoundEffect>("Audio/Land");
            sfxPickup = Content.Load<SoundEffect>("Audio/Pickup");
            sfxSelect = Content.Load<SoundEffect>("Audio/Select");
            sfxHurt = Content.Load<SoundEffect>("Audio/Hurt");
            sfxWaterLoop = Content.Load<SoundEffect>("Audio/WaterLoop");
            sfxWin = Content.Load<SoundEffect>("Audio/Win");

            //Load fonts
            font = Content.Load<SpriteFont>("Fonts/Font");
            titleFont = Content.Load<SpriteFont>("Fonts/TitleFont");
            hudFont = Content.Load<SpriteFont>("Fonts/HudFont");



            //Construct audio holders so we can reference soundeffects through GameManager
            SoundEffect[] clips = { sfxHover, sfxHurt, sfxJump, sfxLand, sfxPickup, sfxSelect, sfxWaterLoop, sfxWin };
            foreach (SoundEffect clip in clips)
            {
                int start = clip.Name.LastIndexOf("/") + 1;
                string name = clip.Name.Substring(start);
                SFXWrapper wrapper = new SFXWrapper(name, clip);
            }

            //Play background music
            MediaPlayer.Play(songBg);
            MediaPlayer.IsRepeating = true;



            Color buttonBackColor = new Color(50, 80, 130);
            int bWidth = 400, bHeight = 100;

            //Create the Main Menu
            Background bMainMenu = new Background("bgMainMenu", texBG1, new Rectangle(0, 0, TargetWidth, TargetHeight), GameState.MainMenu);

            //Main Menu Substate
            UIText title = new UIText("Title", "Current", titleFont, Anchor.UpperMiddle, SortingMode.None, GameState.MainMenu, Point.Zero, Color.White);

            UIButton play = new UIButton("PlayB", "Play", font, texBlock, Anchor.CenterMiddle, SortingMode.Below, GameState.MainMenu, Point.Zero, Color.White, buttonBackColor, bWidth, bHeight);
            UIButton quit = new UIButton("QuitB", "Quit", font, texBlock, Anchor.CenterMiddle, SortingMode.Below, GameState.MainMenu, new Point(0, 50), Color.White, buttonBackColor, bWidth, bHeight);

            //Setup button delegates
            play.Click += LoadCurrentLevel;

            quit.Click += () =>
            {
                Exit();
            };

            //Setup HUD
            HealthBar bar = new HealthBar("HealthBar", texHealth, new Point(100, 66));
            Score score = new Score("Score", hudFont, Anchor.UpperRight, SortingMode.None, GameState.Game, Point.Zero, Color.White);




            //Setup the pause menu
            UIText pauseText = new UIText("pauseText", "PAUSED", font, Anchor.UpperMiddle, SortingMode.Below, GameState.Game, Point.Zero, Color.White);
            pauseText.ActiveGameplayState = GameplayState.Paused;
            UIButton pauseResumeButton = new UIButton("pauseResumeButton", "Resume", font, texBlock, Anchor.CenterMiddle, SortingMode.Below, GameState.Game, new Point(0, 0), Color.White, buttonBackColor);
            pauseResumeButton.ActiveGameplayState = GameplayState.Paused;
            UIButton pauseMainMenuButton = new UIButton("pauseMainMenuButton", "Main Menu", font, texBlock, Anchor.CenterMiddle, SortingMode.Below, GameState.Game, new Point(0, 50), Color.White, buttonBackColor);
            pauseMainMenuButton.ActiveGameplayState = GameplayState.Paused;



            //Pause menu delegates
            pauseResumeButton.Click += () =>
            {
                GameManager.gameplayState = GameplayState.Normal;
            };


            pauseMainMenuButton.Click += LoadMainMenu;


            //Setup the gameover menu
            UIText gameoverText = new UIText("GameoverText", "You have died.", titleFont, Anchor.CenterMiddle, SortingMode.Below, GameState.Game, Point.Zero, Color.White);
            gameoverText.ActiveState = GameState.Game;
            gameoverText.Deactivate();

            UIText gameoverInstr = new UIText("GameoverInstr", "Press the jump button to respawn.", font, Anchor.LowerMiddle, SortingMode.Below, GameState.Game, Point.Zero, Color.White);
            gameoverInstr.ActiveState = GameState.Game;
            gameoverInstr.Deactivate();


            //Setup win level message
            UIText winText = new UIText("WinText", "Current completed the level!", titleFont, Anchor.CenterMiddle, SortingMode.None, GameState.Game, Point.Zero, Color.White);
            winText.ActiveState = GameState.Game;
            winText.Deactivate();


            //Setup win game message
            UIText winGameText = new UIText("WinGameText", "Current was victorious!", titleFont, Anchor.CenterMiddle, SortingMode.None, GameState.Game, Point.Zero, Color.White);
            winGameText.ActiveState = GameState.Game;
            winGameText.Deactivate();


            //Setup win level buttons
            UIButton winMainMenuButton = new UIButton("WinMainMenuButton", "Back to Main Menu", font, texBlock, Anchor.LowerMiddle, SortingMode.None, GameState.Game, Point.Zero, Color.White, buttonBackColor);
            winMainMenuButton.ActiveState = GameState.Game;
            winMainMenuButton.Deactivate();

            //Next level button (part of win menu)
            UIButton winNextButton = new UIButton("WinNextButton", "Next Level", font, texBlock, Anchor.LowerMiddle, SortingMode.None, GameState.Game, Point.Zero, Color.White, buttonBackColor);
            winNextButton.ActiveState = GameState.Game;
            winNextButton.Deactivate();

            //Back to main menu logic
            winMainMenuButton.Click += () =>
            {
                LoadMainMenu();
                winMainMenuButton.Deactivate();
                winNextButton.Deactivate();
                winText.Deactivate();
                winGameText.Deactivate();

                if (GameManager.CompletedAllLevels)
                    GameManager.CurrentLevel = 0;
            };

            winNextButton.Click += () =>
            {
                LoadCurrentLevel();
            };

            UIManager.OrganizeObjects();
        }


        /// <summary>
        /// Load the main menu.
        /// </summary>
        protected void LoadMainMenu()
        {
            GameManager.gameplayState = GameplayState.Normal;
            GameManager.gameState = GameState.MainMenu;
            GameManager.mainMenuState = MainMenuState.MainMenu;
            GameManager.ResetAll();
        }

        /// <summary>
        /// Loads the current level
        /// </summary>
        protected void LoadCurrentLevel()
        {
            //Remove unnecesary objects.
            GameManager.RemoveLevel();
            //Load current level
            LoadLevel(GameManager.CurrentLevel, "Text/Level" + GameManager.CurrentLevel + ".txt");
            //Setup level bounds
            GameManager.OnLoadComplete();
            //Jump into gameplay
            GameManager.gameState = GameState.Game;
        }




        /// <summary>
        /// Load all required assets for given level (starting with zero).
        /// </summary>
        /// <param name="level">0-based level index</param>
        /// <param name="levelFile">Path to level txt file (With extension) </param>
        public void LoadLevel(int level, string levelFile)
        {
            if (level == 0)
            {   

                //Load in the Background
                Background b = new Background("bg1", texBG1, new Rectangle(0, 0, TargetWidth, TargetHeight), GameState.Game);

                //Generate tiles
                GenerateTiles(levelFile);

                //Create the player
                Player player = new Player("Current", texCurSwim, new Rectangle(100, 0, 100, 100));
                Animate playerSwim = new Animate(texCurSwim, 12, 1, Animate.ONESIXTIETHSECPERFRAME * 10, player);
                player.AddAnimation(playerSwim);


                //Enemies
                Catfish c1 = new Catfish("Catfish1", texCatfish, new Rectangle(1997, 1000, 200, 100));
                Catfish c2 = new Catfish("Catfish2", texCatfish, new Rectangle(2500, 1000, 200, 100));


                //Drop in a goal
                Goal goal = new Goal("Goal1", texBlock, new Rectangle(5655, -244, 100, 300));

                //Create the Camera
                MainCamera = new Camera("MainCamera", new Rectangle(0, 0, 0, 0), player);


                //Add pickups
                ScorePickup scorePickup = new ScorePickup("ScorePickup1", texBlock, new Rectangle(1063, 505, 100, 100), 10);
                HealthPickup healthPickup = new HealthPickup("HealthPickup1", texBlock, new Rectangle(1391, 55, 100, 100), 1);
                healthPickup.DrawColor = Color.Red;

                //Add a checkpoint
                CheckPoint checkPoint = new CheckPoint("Checkpoint", texBlock, new Rectangle(2270, -400, 50, 100));
            }
        }

        /// <summary>
        /// Helper method to generate tiles from path
        /// </summary>
        /// <param name="path"></param>
        private void GenerateTiles(string path)
        {

            Dictionary<Point, GameObject> tileDirectory = new Dictionary<Point, GameObject>();

            //Parse the level file
            List<SaveTile> tiles = GameManager.ParseLevel(path);
            //Load in the level tiles
            int numWater = 0, numPlatforms = 0;
            foreach (SaveTile tile in tiles)
            {
                string type = GameManager.TextureMapping[tile.TextureName];
                Texture2D tex = Content.Load<Texture2D>("textures/tiles/" + tile.TextureName);                                      
                Rectangle loc = new Rectangle((int)(tile.X * xRatio), (int)(tile.Y * YRatio), (int)(TileWidth * xRatio), (int)(TileHeight * YRatio ));

                Point check = new Point(loc.X, loc.Y);
                switch (type)
                {
                    case "Water":
                        Water w = new Water("Water" + numWater, tex, loc, Vector2.Zero);
                        //Weed out duplicates
                        if (tileDirectory.ContainsKey(check))
                        {
                            tileDirectory[check].Deactivate();
                            tileDirectory[check] = w;
                        }
                        else
                        {
                            tileDirectory.Add(new Point(loc.X, loc.Y), w);
                        }

                        numWater++;
                        break;
                    case "Platform":
                        Platform p = new Platform("Platform" + numPlatforms, tex, loc);

                        //Weed out duplicates
                        if (tileDirectory.ContainsKey(check))
                        {
                            tileDirectory[check].Deactivate();
                            tileDirectory[check] = p;
                        }
                        else
                        {
                            tileDirectory.Add(new Point(loc.X, loc.Y), p);
                        }


                        numPlatforms++;
                        break;
                }
            }
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

            if (!GameManager.IsLoading)
            {


                UpdateObjects(gameTime);

                //GameManager requires its own update
                GameManager.Update(gameTime);


                //InputManager requires its own Update -- Make sure this is the last update call
                InputManager.Update(gameTime);


                fps = 1.0f / gameTime.ElapsedGameTime.TotalSeconds;

                base.Update(gameTime);
            }

        }

        /// <summary>
        /// Helper method to update all objects 
        /// </summary>
        protected void UpdateObjects(GameTime gameTime)
        {
            //Make a copy so that if values of gamemanager change, we don't get an exception for iterating over a changing dictionary
            Dictionary<string, GameObject> copy = new Dictionary<string, GameObject>(GameManager.GetAll());
            foreach (GameObject g in copy.Values)
            {
                g.Update(gameTime);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (GameManager.IsLoading)
                return;


            spriteBatchBG.Begin();
            foreach (Background b in GameManager.Backgrounds)
            {
                b.Draw(gameTime, spriteBatchBG);
            }
            spriteBatchBG.End();

            if (MainCamera != null)
            {
                spriteBatchGameplay.Begin(transformMatrix: MainCamera.TransformMatrix);
                //Draw all the gameobjects using the translation matrix
                foreach (GameObject g in GameManager.NonUIObjects)
                {
                    g.Draw(gameTime, spriteBatchGameplay);
                }
                spriteBatchGameplay.End();
            }



            spriteBatchUI.Begin();
                //Draw all the UI objects normally
                foreach (UIObject ui in GameManager.UIObjects)
                {
                    ui.Draw(gameTime, spriteBatchUI);
                }

           // Player p = GameManager.Get("Current") as Player;
           // if (p != null)
           //     spriteBatchUI.DrawString(font, p.Location.X + ", " + p.Location.Y, new Vector2(0, 50), Color.White);


            spriteBatchUI.End();


            base.Draw(gameTime);
        }
    }
}
