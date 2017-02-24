using Microsoft.Xna.Framework;
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
    /// </summary>
    abstract class GameObject
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Initializes a GameObject
        /// </summary>
        public GameObject(string name)
        {
            Name = name;
            Texture = null;
        }
        /// <summary>
        /// Initializes a GameObject
        /// </summary>
        /// <param name="texture">The texture to use for this Gameobject</param>
        public GameObject(string name, Texture2D texture)
        {
            Name = name;
            Texture = texture;
        }

        /// <summary>
        /// What do to every frame
        /// </summary>
        /// <param name="gameTime">gameTime</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// What to draw each frame
        /// </summary>
        /// <param name="gameTime">gameTime</param>
        /// <param name="spriteBatch">Active spriteBatch</param>
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
