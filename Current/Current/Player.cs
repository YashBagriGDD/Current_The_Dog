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
        IsJumping,
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


    class Player : CollidableObject
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
        private Vector2 jumpVelocity = new Vector2(0, -20);
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
            state = PlayerState.OnLand;
            direction = Direction.Idle;

            //For the sake of physics
            Acceleration = new Vector2(0, 0f);
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
                case PlayerState.IsJumping:
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
            if (InputManager.GetButton("Right"))
            {
                Velocity.X = speed;
            }
            else if (InputManager.GetButton("Left"))
            {
                Velocity.X = -speed;
            }
            else
            {
                Velocity.X = 0;
            }
            //Handle jumping, but ONLY on land
            if (InputManager.GetButtonDown("Jump") && state == PlayerState.OnLand)
            {
                state = PlayerState.IsJumping;
                //Start pulling the player down
                Acceleration = airAcceleration;
                //But give them an initial upward velocity
                Velocity = jumpVelocity;
            }
        }

        //What to do for collisions
        protected override void HandleCollisionEnter(object sender, EventArgs e)
        {
            //Cast to a Collider
            Collider c = (Collider)sender;
            
            if (c.Host is Platform)
            {
                Acceleration = Vector2.Zero;
                Velocity = Vector2.Zero;
                state = PlayerState.OnLand;
            }
        }
        protected override void HandleCollisionExit(object sender, EventArgs e)
        {

        }
    }
}
