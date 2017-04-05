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

        public MainMenuState ActiveMainMenuState { get; set; }
            = MainMenuState.MainMenu;

        public GameplayState ActiveGameplayState { get; set; }
            = GameplayState.Normal;


        /// <summary>
        /// Used to determine if this is the state to draw and update everything
        /// </summary>
        protected bool InTheRightState
        {
            get
            {
                if (ActiveState != GameManager.gameState)
                    return false;
                switch (ActiveState)
                {
                    case GameState.Game:
                        if (ActiveGameplayState != GameManager.gameplayState)
                            return false;
                        break;
                    case GameState.MainMenu:
                        if (ActiveMainMenuState != GameManager.mainMenuState)
                            return false;
                        break;
                    case GameState.GameOver:
                        break;
                    default:
                        break;
                }
                return true;
            }
        }

        /// <summary>
        /// Dictionary of all animations, referenced by texture name
        /// </summary>
        protected Dictionary<string, Animate> AnimationData { get; set; }

        protected string currentAnimation { get; set; }

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

            //Setup Animation
            Animate defaultAnim = new Animate(Texture, 1, 1, Animate.ONESIXTIETHSECPERFRAME, this);
            AnimationData = new Dictionary<string, Animate>();
            AnimationData.Add(texture.Name, defaultAnim);
            currentAnimation = texture.Name;
        }

        /// <summary>
        /// Change to a new animation and play it.
        /// The Animation MUST have been added before you try to play it by name!
        /// </summary>
        /// <param name="animationName">Name of the new animation</param>
        public void ChangeAnimation(string animationName)
        {
            if (!AnimationData.ContainsKey(animationName))
            {
                Console.WriteLine("Animation not found!");
            }
            else
            {
                currentAnimation = animationName;
                AnimationData[currentAnimation].Reset();
            }
        }
        

        /// <summary>
        /// Add a new animation that will be able to be played by name
        /// </summary>
        /// <param name="newAnimation"></param>
        public void AddAnimation(Animate newAnimation)
        {
            if (AnimationData.ContainsKey(newAnimation.Name))
                AnimationData[newAnimation.Name] = newAnimation;
            else
                AnimationData.Add(newAnimation.Name, newAnimation);
        }

        /// <summary>
        /// What do to every frame
        /// </summary>
        /// <param name="gameTime">gameTime</param>
        public virtual void Update(GameTime gameTime)
        {
            if (!InTheRightState)
                return;

            //Change the velocity by acceleration
            Velocity.X += Acceleration.X;
            Velocity.Y += Acceleration.Y;

            //Then change the displacement by velocity
            Location.X += (int)(Velocity.X * gameTime.ElapsedGameTime.TotalSeconds * 100);
            Location.Y += (int)(Velocity.Y * gameTime.ElapsedGameTime.TotalSeconds * 100);

            //Make sure this can be animated 
            if (AnimationData == null)
                return;
            if (AnimationData.ContainsKey(currentAnimation))
                AnimationData[currentAnimation].Update(gameTime);
        }

        /// <summary>
        /// By default, draws this GameObject
        /// </summary>
        /// <param name="gameTime">gameTime</param>
        /// <param name="spriteBatch">Active spriteBatch</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!InTheRightState)
                return;

            if (AnimationData == null || !AnimationData.ContainsKey(currentAnimation))
                spriteBatch.Draw(Texture, destinationRectangle: Location, color: DrawColor, effects: SpriteFX);
            else
                AnimationData[currentAnimation].Draw(spriteBatch);
        }

    }
}
