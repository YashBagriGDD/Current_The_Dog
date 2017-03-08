using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Current
{
    enum PlayerState
    {
        OnLand,
        InAir,
        InWater,
        IsDead
    }

    enum Direction
    {
        Idle=0,
        Right=1,
        Left=2,
        Up=4,
        UpRight=5,
        UpLeft=6,
        Down=8,
        DownRight = 9,
        DownLeft = 10
    }


    class Player : ComplexCollidableObject
    {

        //Player Health
        public int Health { get; set; }
        //Rate at which player will move
        public int MoveSpeed { get; set; }
        //How strong is the player??
        public int Strength { get; set; }



        //General state enum
        public PlayerState state { get; set; }
        //Specialized enum for direction
        public Direction direction { get; set; }

        //The current time
        private GameTime gameTime;


        //The initial velocity for jumping
        private Vector2 jumpVelocity = new Vector2(0, -30);
        //The acceleration used in the air
        private Vector2 airAcceleration = new Vector2(0, 1);


        
        /// <summary>
        /// Initialize player
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tex"></param>
        /// <param name="speed"></param>
        public Player(string name, Texture2D tex, Rectangle location, int speed) : base(name, tex, location)
        {
            //Setup various states and attributes
            MoveSpeed = speed;
            //Assume in air to begin with
            state = PlayerState.InAir;
            direction = Direction.Idle;

            //For the sake of physics
            Acceleration = airAcceleration;
            Velocity = new Vector2(0, 0);
            
            
        }
        
        /// <summary>
        /// Draws the Player
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// Main Update loop
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;

            //The main finite state machine. 
            switch (state)
            {
                case PlayerState.OnLand:
                    Move(MoveSpeed);
                    break;
                case PlayerState.InAir:
                    //Move at reduced speed
                    Move(3*MoveSpeed/4);
                    Jump();
                    break;
                case PlayerState.InWater:
                    break;
                case PlayerState.IsDead:
                    break;
                default:
                    break;
            }
            base.Update(gameTime);
        }


        public void Die()
        {
        }
        public void Hurt()
        {
        }

        public void Jump()
        {
   
        }
        /// <summary>
        /// Manage movement for a variable speed
        /// Jump also activated here, but only on Land
        /// </summary>
        /// <param name="speed"></param>
        public void Move(int speed)
        {
            if (InputManager.GetButton("Right") && !CollRight.CollidingWith<Platform>())
            {
                Velocity.X = speed;
                SpriteFX = SpriteEffects.None;
                direction = Direction.Right;
            }
            else if (InputManager.GetButton("Left") && !CollLeft.CollidingWith<Platform>())
            {
                Velocity.X = -speed;
                SpriteFX = SpriteEffects.FlipHorizontally;
                direction = Direction.Left;

            }
            else
            {
                Velocity.X = 0;
            }
            //Handle jumping, but ONLY on land
            if (InputManager.GetButtonDown("Jump") && state == PlayerState.OnLand)
            {
                state = PlayerState.InAir;
                //Start pulling the player down
                Acceleration = airAcceleration;
                //But give them an initial upward velocity
                Velocity = jumpVelocity;
            }
        }

        /// <summary>
        /// What do for collisions
        /// </summary>
        /// <param name="sender">Who am I colliding with?</param>
        /// <param name="e">Ignore this</param>
        /// If you need more details, use CollAbove, CollLeft, etc. 
        /// Alternatively, here are some useful comparisions: 
        /// <example>
        /// if (Location.Y < c.Host.Location.Y) Is true when colliding with something below
        /// if (Location.Y > c.Host.Location.Y) Is true when colliding with something above
        /// if (Location.X < c.Host.Location.X) Is true when colliding with something on the right
        /// if (Location.X > c.Host.Location.X) Is true when colliding with something on the left
        /// </example>
        protected override void HandleCollisionEnter(object sender, EventArgs e)
        {
            //Cast to a Collider
            Collider other = (Collider)sender;

            switch (state)
            {
                case PlayerState.OnLand:
                    break;
                case PlayerState.InAir:
                    //Stop on a platform
                    if (other.Host is Platform)
                    {
                        //When colliding below, stop
                        if (CollBelow.CollidingWith(other.Host))
                        {
                            Acceleration = Vector2.Zero;
                            Velocity.Y = 0;
                            state = PlayerState.OnLand;
                        }
                        //When colliding above, reset y velocity
                        else if (CollAbove.CollidingWith(other.Host))
                        {
                            Velocity.Y = 0;
                        }
                        
                    }
                    break;
                case PlayerState.InWater:
                    break;
                case PlayerState.IsDead:
                    break;
                default:
                    break;
            }


        }
        /// <summary>
        /// What do when a collision ends
        /// </summary>
        /// <param name="sender">Who WAS I colliding with?</param>
        /// <param name="e">Ignore this</param>
        protected override void HandleCollisionExit(object sender, EventArgs e)
        {
            //Cast to a Collider
            Collider other = (Collider)sender;

            switch (state)
            {
                case PlayerState.OnLand:
                    //Stop on a platform
                    if (other.Host is Platform)
                    {
                        if (!CollBelow.CollidingWith(other.Host))
                        {
                            Acceleration = airAcceleration;
                            state = PlayerState.InAir;
                        }
                    }
                    break;
                case PlayerState.InAir:
                    break;
                case PlayerState.InWater:
                    break;
                case PlayerState.IsDead:
                    break;
                default:
                    break;
            }
        }
    }
}
