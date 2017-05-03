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

        /// <summary>
        /// Origin to draw at
        /// </summary>
        public Vector2? Origin { get; protected set; } = null;
        /// <summary>
        /// Rotation
        /// </summary>
        public float Rotation { get; protected set; } = 0;
        //Should this sprite be flipped in any direction?
        public SpriteEffects SpriteFX { get; set; }
            = SpriteEffects.None;

        //The states are virtual so that objects, like UIButton can override it.

        //The state in which this object is drawn and updated
        public virtual GameState ActiveState { get; set; }
            = GameState.Game;

        public virtual MainMenuState ActiveMainMenuState { get; set; }
            = MainMenuState.MainMenu;

        public virtual GameplayState ActiveGameplayState { get; set; }
            = GameplayState.Normal;

        /// <summary>
        /// Spawn Location for this object
        /// You can change this, and have the object move to this location on reset
        /// </summary>
        public Rectangle SpawnLocation { get; set; }


        /// <summary>
        /// The actual location that this object was loaded that.
        /// This cannot be changed.
        /// </summary>
        public Rectangle LoadLocation { get; private set; }

        //Initial states
        protected GameState initGameState;
        protected GameplayState initGameplayState;
        protected MainMenuState initMainmenuState;

        
        /// <summary>
        /// Should this gameobject update?
        /// </summary>
        public bool CanUpdate { get; set; } = true;


        /// <summary>
        /// Used to determine if this is active, based on the GameState is the state to draw and update everything
        /// </summary>
        public bool Active
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

        //The last state this instance was in
        protected GameState previousState = GameState.None;

        /// <summary>
        /// Should this object draw no matter what and ignore optimizations?
        /// </summary>
        public bool AlwaysDraw { get; set; } = false;

        /// <summary>
        /// Initializes a GameObject
        /// </summary>
        public GameObject(string name, Rectangle location)
        {
            Name = name;
            Texture = null;
            GameManager.Add(Name, this);


            //Save start values
            Location = location;
            LoadLocation = Location;
            SpawnLocation = Location;
            initGameplayState = ActiveGameplayState;
            initGameState = ActiveState;
            initMainmenuState = ActiveMainMenuState;
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



            //Save start values
            Location = location;
            SpawnLocation = Location;
            LoadLocation = location;
            initGameplayState = ActiveGameplayState;
            initGameState = ActiveState;
            initMainmenuState = ActiveMainMenuState;

            //Setup Animation
            Animate defaultAnim = new Animate(Texture, 1, 1, Animate.ONESIXTIETHSECPERFRAME, this);
            AnimationData = new Dictionary<string, Animate>();
            AnimationData.Add(texture.Name, defaultAnim);
            currentAnimation = texture.Name;
        }


        /// <summary>
        /// Is this object and the provided object equal in terms of states?
        /// </summary>
        public bool AreStatesEqual(GameObject other)
        {
            return (ActiveState == other.ActiveState && ActiveGameplayState == other.ActiveGameplayState && ActiveMainMenuState == other.ActiveMainMenuState);
        }

        /// <summary>
        /// Decactivate this instance. This means no update, draw, or collision methods will ever be called.
        /// </summary>
        public void Deactivate()
        {
            //Assuming we're not already deactivated
            if (ActiveState != GameState.None)
            {
                previousState = ActiveState;
                ActiveState = GameState.None;
            }

        }

        /// <summary>
        /// Activate this instance. Only necessary to call if this has been deactivated previously.
        /// </summary>
        public void Activate()
        {
            if (previousState != GameState.None)
            {
                ActiveState = previousState;
            }

        }


        /// <summary>
        /// Resets this GameObject to its initial location, and resets states to default.
        /// </summary>
        public virtual void Reset()
        {
            Location = LoadLocation;
            SpawnLocation = LoadLocation;
            if (!CanUpdate)
                CanUpdate = true;
        }

        /// <summary>
        /// Respawns this gameobject at SpawnLocation, and resets states to default.
        /// </summary>
        public virtual void Respawn()
        {
            Location = SpawnLocation;
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
                //If we change to the same animation, no need to change anything
                if (currentAnimation == animationName)
                    return;
                currentAnimation = animationName;
                AnimationData[currentAnimation].Reset();
            }
        }
        

        /// <summary>
        /// Add a new animation that can be able to be played by name
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
            if (!Active || !CanUpdate)
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
        /// <param name="depth">Optional depth specification</param>
        /// <param name="origin">Sprite origin point. Default null</param>
        /// <param name="rotation">Rotation</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Don't draw if outside camera's bounds
            Camera cam = GameManager.Get("MainCamera") as Camera;
            if (cam != null && !AlwaysDraw)
            {
                if (!cam.Bounds.Intersects(Location))
                    return;
            }


            if (!Active || Texture == null)
                return;

            if (AnimationData == null || !AnimationData.ContainsKey(currentAnimation))
                spriteBatch.Draw(Texture, destinationRectangle: Location, color: DrawColor, effects: SpriteFX, origin:Origin, rotation:Rotation);
            else
                AnimationData[currentAnimation].Draw(spriteBatch);

        }

    }
}
