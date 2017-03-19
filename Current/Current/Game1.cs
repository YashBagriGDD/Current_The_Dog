using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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


        SpriteFont font;

        //Window sizes
        public const int WINDOW_WIDTH = 1920;
        public const int WINDOW_HEIGHT = 1080;

        //Debugging variables
        private double fps = 0;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Set window size
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
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

            Texture2D texPlayer = Content.Load<Texture2D>("Textures/dogTemp");
            Texture2D texBlock = Content.Load<Texture2D>("Textures/WhiteBlock");
            font = Content.Load<SpriteFont>("Fonts/Font");

            //Load in some demo objects
            Platform plat = new Platform("Platform", texBlock, new Rectangle(0, 1000, 600, 500));
            Platform plat2 = new Platform("Platform2", texBlock, new Rectangle(0, 750, 100, 300));
            Platform plat3 = new Platform("Platform3", texBlock, new Rectangle(1000, 1000, 500, 100));

            Water water = new Water("WaterTest", texBlock, new Rectangle(600, 200, 1000, 600), Vector2.Zero);

            Player player = new Player("Current", texPlayer, new Rectangle(100, 250, 100, 100));

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
