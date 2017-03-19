﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Current
{
    /// <summary>
    /// The base class for all objects in the game
    /// When you instantiate any derived class, it will automatically inherit Location, Velocity, Acceleration, and other useful properties. 
    /// It will also auto-add itself to the list of objects in the GameManager
    /// NOTE ON PHYSICS:
    /// If you'd like gravity implemented, simply adjust the acceleration attribute to a positive number in the y directoin
    /// If you'd prefer not to use any physics, just set velocity and acceleration to Vector2.Zero
    /// </summary>
    abstract class GameObject
    {
        //The position, width, and height
        public Rectangle Location;
        //The texture to draw
        public Texture2D Texture { get; set; }
        //Don't forget to name your object!
        public string Name { get; set; }
        //What color to draw this object
        public Color DrawColor { get; set; }
            = Color.White;
        //Should this sprite be flipped in any direction?
        public SpriteEffects SpriteFX { get; set; }
            = SpriteEffects.None;

        //The state in which this object is drawn and updated
        public GameState ActiveState { get; set; }
            = GameState.Game;


        //Represents the change in displacement per update
        public Vector2 Velocity;
        //Represents the change in velocity per update
        public Vector2 Acceleration;
        private Texture2D tex;

        /// <summary>
        /// Initializes a GameObject
        /// </summary>
        public GameObject(string name, Rectangle location)
        {
            Name = name;
            Texture = null;
            GameManager.Add(Name, this);
            Location = location;
        }
        /// <summary>
        /// Initializes a GameObject
        /// </summary>
        /// <param name="texture">The texture to use for this Gameobject</param>
        public GameObject(string name, Texture2D texture, Rectangle location)
        {
            Name = name;
            Texture = texture;
            GameManager.Add(Name, this);
            Location = location;

        }

        /// <summary>
        /// What do to every frame
        /// </summary>
        /// <param name="gameTime">gameTime</param>
        public virtual void Update(GameTime gameTime)
        {
            if (GameManager.gameState != ActiveState)
                return;

            //Change the velocity by acceleration
            Velocity.X += Acceleration.X;
            Velocity.Y += Acceleration.Y;

            //Then change the displacement by velocity
            Location.X += (int)(Velocity.X * gameTime.ElapsedGameTime.TotalSeconds * 100);
            Location.Y += (int)(Velocity.Y * gameTime.ElapsedGameTime.TotalSeconds * 100);
        }

        /// <summary>
        /// By default, draws this GameObject
        /// </summary>
        /// <param name="gameTime">gameTime</param>
        /// <param name="spriteBatch">Active spriteBatch</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (GameManager.gameState != ActiveState)
                return;

            spriteBatch.Draw(Texture, destinationRectangle: Location, color: DrawColor, effects: SpriteFX);
        }
    }
}
