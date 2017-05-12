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


        SpriteFont font, titleFont, hudFont, teamFont;

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

        //BG audio
        public Song songBg;

        /// <summary>
        /// Holds all textures
        /// </summary>
        public static Dictionary<string, Texture2D> Textures;
        /// <summary>
        /// Holds all Sound Effects
        /// </summary>
        public static Dictionary<string, SoundEffect> SoundEffects;

        
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



            //Load data
            GameManager.LoadSavedData();

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
            spriteBatchGameplay = new SpriteBatch(GraphicsDevice);
            spriteBatchUI = new SpriteBatch(GraphicsDevice);
            spriteBatchBG = new SpriteBatch(GraphicsDevice);

            //Load textures
            Textures = new Dictionary<string, Texture2D>
            {
                {"CurrentSwim", Content.Load<Texture2D>("Textures/Current/CurrentSwim")},
                {"CurrentWalk", Content.Load<Texture2D>("Textures/Current/CurrentWalk") },
                {"CurrentIdle", Content.Load<Texture2D>("Textures/Current/CurrentIdle") },
                {"CurrentJump", Content.Load<Texture2D>("Textures/Current/CurrentJump") },
                {"WhiteBlock",  Content.Load<Texture2D>("Textures/WhiteBlock")},
                {"Background", Content.Load<Texture2D>("Textures/Backgrounds/Sunset") },
                {"Crossbone", Content.Load<Texture2D>("Textures/HUD/Crossbone") },
                {"Catfish", Content.Load<Texture2D>("Textures/Enemies/Catfish") },
                {"Landcat", Content.Load<Texture2D>("Textures/Enemies/Landcat") },
                {"deep water tile", Content.Load<Texture2D>("Textures/Tiles/deep water tile")},
                {"grass tile", Content.Load<Texture2D>("Textures/Tiles/grass tile")},
                {"grass to sand tile", Content.Load<Texture2D>("Textures/Tiles/grass to sand tile")},
                {"sand tile", Content.Load<Texture2D>("Textures/Tiles/sand tile")},
                {"shore tile", Content.Load<Texture2D>("Textures/Tiles/shore tile")},
                {"upper water tile", Content.Load<Texture2D>("Textures/Tiles/upper water tile")},
                {"Button", Content.Load<Texture2D>("Textures/HUD/Button") },
                {"Laser", Content.Load<Texture2D>("Textures/Items/Laser") },
                {"Checkpoint", Content.Load<Texture2D>("Textures/Items/Checkpoint") },
                {"CheckpointHit", Content.Load<Texture2D>("Textures/Items/CheckpointHit") },
                {"HealthPickup", Content.Load<Texture2D>("Textures/Items/HealthPickup") },
                {"LaserPickup", Content.Load<Texture2D>("Textures/Items/LaserPickup") },
                {"ScorePickup", Content.Load<Texture2D>("Textures/Items/ScorePickup") }
                
            };



    
            //Load Background Music
            songBg = Content.Load<Song>("Audio/Current");



            //Load Sound Effects
            SoundEffects = new Dictionary<string, SoundEffect>
            {
                {"Hover", Content.Load<SoundEffect>("Audio/Hover") },
                {"Hurt", Content.Load<SoundEffect>("Audio/Hurt") },
                {"Jump", Content.Load<SoundEffect>("Audio/Jump") },
                {"Land", Content.Load<SoundEffect>("Audio/Land") },
                {"Pickup", Content.Load<SoundEffect>("Audio/Pickup")},
                {"Select",  Content.Load<SoundEffect>("Audio/Select") },
                {"WaterLoop",  Content.Load<SoundEffect>("Audio/WaterLoop")},
                {"Win", Content.Load<SoundEffect>("Audio/Win")},
                {"Laser", Content.Load<SoundEffect>("Audio/Laser") }
            };


            //Load fonts
            font = Content.Load<SpriteFont>("Fonts/Font");
            titleFont = Content.Load<SpriteFont>("Fonts/TitleFont");
            hudFont = Content.Load<SpriteFont>("Fonts/HudFont");
            teamFont = Content.Load<SpriteFont>("Fonts/TeamFont");


            //Construct audio holders so we can reference soundeffects through GameManager
            foreach (string clipName in SoundEffects.Keys)
            {
                SFXWrapper wrapper = new SFXWrapper(clipName, SoundEffects[clipName]);
            }

            //Play background music
            MediaPlayer.Play(songBg);
            MediaPlayer.IsRepeating = true;



            Color buttonBackColor = new Color(50, 80, 130);
            int bWidth = 400, bHeight = 100;

            //Create the Main Menu
            UIImage bMainMenu = new UIImage("bgMainMenu", Textures["Background"], new Point( TargetWidth, TargetHeight), Anchor.UpperLeft, SortingMode.None, GameState.MainMenu, Point.Zero, Color.White);

            //Main Menu Substate
            UIText title = new UIText("Title", "Current", titleFont, Anchor.UpperMiddle, SortingMode.None, GameState.MainMenu, Point.Zero, Color.White);

            UIButton play = new UIButton("PlayB", "Play", font, Textures["Button"], Anchor.CenterMiddle, SortingMode.Below, GameState.MainMenu, Point.Zero, Color.Black, Color.White, bWidth, bHeight);
            UIButton quit = new UIButton("QuitB", "Quit", font, Textures["Button"], Anchor.CenterMiddle, SortingMode.Below, GameState.MainMenu, new Point(0, 50), Color.Black, Color.White, bWidth, bHeight);

            //Tean
            UIText team = new UIText("Team", "A Guppy Puppy Studios Creation", teamFont, Anchor.LowerLeft, SortingMode.None, GameState.MainMenu, Point.Zero, Color.Gray);
            //Setup button delegates
            play.Click += LoadCurrentLevel;

            quit.Click += () =>
            {
                Exit();
            };

            //Setup HUD
            HealthBar bar = new HealthBar("HealthBar", Textures["Crossbone"], new Point(100, 66));
            Score score = new Score("Score", hudFont, Anchor.UpperRight, SortingMode.Below, GameState.Game, Point.Zero, Color.White);
            LevelDisplay curlevel = new LevelDisplay("LevelDisplay", hudFont, Anchor.UpperRight, SortingMode.Below, GameState.Game, Point.Zero, Color.White);



            //Setup the pause menu
            UIImage bgPause = new UIImage("bgPause", Textures["Background"], new Point(TargetWidth, TargetHeight), Anchor.UpperLeft, SortingMode.None, GameState.Game, Point.Zero, Color.White);
            bgPause.ActiveGameplayState = GameplayState.Paused;


            UIText pauseText = new UIText("pauseText", "PAUSED", font, Anchor.UpperMiddle, SortingMode.Below, GameState.Game, Point.Zero, Color.White);
            pauseText.ActiveGameplayState = GameplayState.Paused;
            UIButton pauseResumeButton = new UIButton("pauseResumeButton", "Resume", font, Textures["Button"], Anchor.CenterMiddle, SortingMode.Below, GameState.Game, new Point(0, 0), Color.Black, Color.White);
            pauseResumeButton.ActiveGameplayState = GameplayState.Paused;
            UIButton pauseMainMenuButton = new UIButton("pauseMainMenuButton", "Main Menu", font, Textures["Button"], Anchor.CenterMiddle, SortingMode.Below, GameState.Game, new Point(0, 50), Color.Black, Color.White);
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
            UIText winText = new UIText("WinText", "Current completed the level!", titleFont, Anchor.CenterMiddle, SortingMode.None, GameState.Game, Point.Zero, Color.Black);
            winText.ActiveState = GameState.Game;
            winText.Deactivate();

            HighscoreDisplay highscore = new HighscoreDisplay("Highscore", font, Anchor.LowerRight, SortingMode.None, GameState.Game, Point.Zero, Color.White);
            highscore.ActiveState = GameState.Game;
            highscore.Deactivate();

            //Setup win game message
            UIText winGameText = new UIText("WinGameText", "Current Completed His Journey!", titleFont, Anchor.CenterMiddle, SortingMode.None, GameState.Game, Point.Zero, Color.White);
            winGameText.ActiveState = GameState.Game;
            winGameText.Deactivate();


            //Setup win level buttons
            UIButton winMainMenuButton = new UIButton("WinMainMenuButton", "Back to Main Menu", font, Textures["Button"], Anchor.LowerLeft, SortingMode.None, GameState.Game, Point.Zero, Color.Black, Color.White);
            winMainMenuButton.ActiveState = GameState.Game;
            winMainMenuButton.Deactivate();

            //Next level button (part of win menu)
            UIButton winNextButton = new UIButton("WinNextButton", "Next Level", font, Textures["Button"], Anchor.LowerRight, SortingMode.None, GameState.Game, Point.Zero, Color.Black, Color.White);
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
                highscore.Deactivate();

                if (GameManager.CompletedAllLevels)
                    GameManager.CurrentLevel = 0;
            };

            winNextButton.Click += () =>
            {
                winMainMenuButton.Deactivate();
                winNextButton.Deactivate();
                winText.Deactivate();
                winGameText.Deactivate();
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
            //GameManager.ResetAll();
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
            //Load in the Background
            ParallaxBackground b = new ParallaxBackground("ParallaxBG1", Textures["Background"], new Rectangle(0, 0, TargetWidth, TargetHeight), GameState.Game, 1, null);

            //Generate tiles
            GenerateTiles(levelFile);


            Player player = null;

            if (level == 0)
            {


                //Enemies
                Catfish c1 = new Catfish("Catfish1", Textures["Catfish"], new Rectangle(1997, 1000, 100, 50));
                Catfish c2 = new Catfish("Catfish2", Textures["Catfish"], new Rectangle(2500, 1000, 100, 50));




                //Drop in a goal
                Goal goal = new Goal("Goal1", Textures["Crossbone"], new Rectangle(5400, -244, 200, 100));

                //Add pickups
                ScorePickup scorePickup = new ScorePickup("ScorePickup1", Textures["ScorePickup"], new Rectangle(1063, 505, 100, 100), 10);
                HealthPickup healthPickup = new HealthPickup("HealthPickup1", Textures["HealthPickup"], new Rectangle(1391, 55, 100, 100), 1);
                healthPickup.DrawColor = Color.Red;

                //Add a checkpoint
                CheckPoint checkPoint = new CheckPoint("Checkpoint", Textures["Checkpoint"], new Rectangle(2270, -400, 50, 100));

                //Add in tutorial text
                WorldText tut1 = new WorldText("Tutorial1", "W,A,S,D = Move", font, new Vector2(200, 200));
                WorldText tut2 = new WorldText("Tutorial2", "Space = Jump", font, new Vector2(200, 300));

                player = new Player("Current", Textures["CurrentIdle"], Textures["Laser"], new Rectangle(100, 0, 100, 100));
            }
            else if (level == 1)
            {
                player = new Player("Current", Textures["CurrentIdle"], Textures["Laser"], new Rectangle(368, 355, 100, 100));
                ScorePickup scorePickup = new ScorePickup("ScorePickup", Textures["ScorePickup"], new Rectangle(-4688, -1144, 100, 100), 10);

                WorldText laserInstr = new WorldText("LaserInstruction", "Shift=Laser", font, new Vector2(550, 200));
                LaserPickup laser = new LaserPickup("LaserPickup", Textures["LaserPickup"], new Rectangle(550,300, 100, 100), 0);

                LandCat landCat = new LandCat("Landcat", Textures["Landcat"], new Rectangle(3337, 55, 200, 125));
                LandCat landCat4 = new LandCat("Landcat4", Textures["Landcat"], new Rectangle(7866, -844, 200, 125));
                LandCat landCat5 = new LandCat("Landcat5", Textures["Landcat"], new Rectangle(8966, -1444, 200, 125));
                Goal goal = new Goal("Goal", Textures["Crossbone"], new Rectangle(14378, -2794, 200, 100));




            }
            else if(level == 2)
            {
                player = new Player("Current", Textures["CurrentIdle"], Textures["Laser"], new Rectangle(200, 205, 100, 100));

                //Add pickups
                ScorePickup scorePickup = new ScorePickup("ScorePickup1", Textures["ScorePickup"], new Rectangle(2500, 50, 100, 100), 10);
                ScorePickup scorePickup2  = new ScorePickup("ScorePickup2", Textures["ScorePickup"], new Rectangle(7500, -2100, 100, 100), 10);

                Goal goal = new Goal("Goal1", Textures["Crossbone"], new Rectangle(7607, -1594, 200, 100));

            }
            else if(level == 3)
            {
                //score blocks
                ScorePickup scorePickup = new ScorePickup("ScorePickup1", Textures["ScorePickup"], new Rectangle(4300, -2077, 100, 100), 10);

                //enemy 
                LandCat kitty = new LandCat("Cat1", Textures["Landcat"], new Rectangle(3744,-94, 200, 125));
                player = new Player("Current", Textures["CurrentIdle"], Textures["Laser"], new Rectangle(238, 205, 100, 100));

                Goal goal = new Goal("Goal1", Textures["Crossbone"], new Rectangle(9555, -2944, 200, 100)); 
                
            }

            //Create the Camera
            MainCamera = new Camera("MainCamera", new Rectangle(0, 0, 0, 0), player);

            //Setup parallax bg
            ParallaxBackground p = (ParallaxBackground)(GameManager.Get("ParallaxBG1"));
            p.Target = player;




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
                Texture2D tex = Textures[tile.TextureName];                                      
                Rectangle loc = new Rectangle((int)(tile.X * xRatio), (int)(tile.Y * YRatio), (int)(TileWidth * xRatio), (int)(TileHeight * YRatio * 1.1f));

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
                //Draw the background(s) first


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
           
            spriteBatchUI.End();


            base.Draw(gameTime);
        }
    }
}
