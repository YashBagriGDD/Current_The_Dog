﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MapEditor {
    //TO DO MAKE EASY ACCESS VARIABLES FOR THE SIZES

    class Tile {
        public float X { get; set; }
        public float Y { get; set; }
        public Texture2D text { get; set; }
    }

    //for saving the name of the Texture and writing it to a text file.
    class SaveTile {
        public float X { get; set; }
        public float Y { get; set; }
        public string TextureName { get; set; }
    }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D currentTexture;
        Texture2D bg;
        int currentX = 0;
        int lvlX = 0;
        int currentY = 0;
        int lvlY = 0;
        int listPosition;
        int tileSize; //int for storing the tile size in one place for easy editing
        List<Tile> platforms = new List<Tile>();
        List<SaveTile> save = new List<SaveTile>();
        List<Texture2D> textures = new List<Texture2D>(); //Add textures into this list for scrolling through them in editor
        //Instantiate all textures here
        Texture2D deepWater;
        Texture2D upWater;
        Texture2D shore;
        Texture2D grassSand;
        Texture2D grass;
        Texture2D sand;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here
            tileSize = 100; //the tile size
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bg = Content.Load<Texture2D>("background");
            listPosition = 0; //int for keeping track of place in list
            //Load Textures here and add them to the list
            deepWater = Content.Load<Texture2D>("deep water tile");
            textures.Add(deepWater);
            upWater = Content.Load<Texture2D>("upper water tile");
            textures.Add(upWater);
            shore = Content.Load<Texture2D>("shore tile");
            textures.Add(shore);
            sand = Content.Load<Texture2D>("sand tile");
            textures.Add(sand);
            grassSand = Content.Load<Texture2D>("grass to sand tile");
            textures.Add(grassSand);
            grass = Content.Load<Texture2D>("grass tile");
            textures.Add(grass);

            currentTexture = textures[listPosition];
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        ButtonState prevButton = Mouse.GetState().LeftButton; //for saving the button state to bottleneck placing tiles when holding down the button
        KeyboardState prev = Keyboard.GetState(); //for saving keyboard state to stop infinite scrolling
        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (Keyboard.GetState().IsKeyDown(Keys.D) && prev.IsKeyUp(Keys.D)) {
                listPosition++;
                if (listPosition >= textures.Count) {
                    listPosition = 0;
                }
                currentTexture = textures[listPosition];
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S) && prev.IsKeyUp(Keys.S)) {
                listPosition--;
                if (listPosition < 0) {
                    listPosition = textures.Count - 1;
                }
                currentTexture = textures[listPosition];
            }
            if (Keyboard.GetState().IsKeyDown(Keys.N) ) {
                Process.Start("notepad.exe");
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && prev.IsKeyUp(Keys.Right)) {
                currentX += tileSize;
                lvlX += tileSize;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && prev.IsKeyUp(Keys.Left)) {
                currentX -= tileSize;
                lvlX -= tileSize;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && prev.IsKeyUp(Keys.Down)) {
                currentY += tileSize;
                lvlY += tileSize;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && prev.IsKeyUp(Keys.Up)) {
                currentY -= tileSize;
                lvlY -= tileSize;
            }


            if (currentX < 0)
                currentX = 0;
            currentX %= GraphicsDevice.Viewport.Bounds.Width * 2;

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && prevButton != ButtonState.Pressed) {
                platforms.Add(new Tile() {
                    //For snapping, change the number to equal the Tile Size
                    X = (Mouse.GetState().X / tileSize * tileSize) + lvlX,
                    Y = (Mouse.GetState().Y /tileSize *tileSize) + lvlY,
                    text = currentTexture
                });
            }

            //Write Tile List to file
            if (Keyboard.GetState().IsKeyDown(Keys.Enter)) {
                save.Clear(); //Clearing list is saving multiple times in a work session
                //Adds every item in the platform list to the save list, gets the name of the texture
                foreach (var item in platforms) {
                    save.Add(new SaveTile { X = item.X, Y = item.Y, TextureName = item.text.Name });
                }
                string json = JsonConvert.SerializeObject(save.ToArray(), Formatting.Indented);
                File.WriteAllText(@"level.txt", json);
            }

            //Remove the most recent tile in platforms array
            if (Keyboard.GetState().IsKeyDown(Keys.Back) && prev.IsKeyUp(Keys.Back)) {
                platforms.Remove(platforms.Last());
            }

            prev = Keyboard.GetState();
            prevButton = Mouse.GetState().LeftButton;
            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(bg, new Rectangle(-currentX, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.Draw(bg, destinationRectangle: new Rectangle(GraphicsDevice.Viewport.Width - currentX, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), effects: SpriteEffects.FlipHorizontally);
            spriteBatch.Draw(bg, new Rectangle(2 * GraphicsDevice.Viewport.Width - currentX, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            spriteBatch.Draw(currentTexture, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, tileSize, tileSize), Color.White);
            foreach (var item in platforms) {
                spriteBatch.Draw(item.text, new Rectangle((int)item.X - lvlX, (int)item.Y - lvlY, tileSize, tileSize), Color.White);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
