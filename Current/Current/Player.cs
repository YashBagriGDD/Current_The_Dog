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

        public int Health { get; set; }
        public int MoveSpeed { get; set; }
        public int Strength { get; set; }

        public Vector2 Velocity;
        public Vector2 Acceleration;

        public PlayerState state { get; set; }
        public Direction direction { get; set; }

        private GameTime gameTime;
        private double storedJumpTime;

        private Vector2 jumpVelocity = new Vector2(0, -20);
        private Vector2 landAcceleration = new Vector2(0, 1);


        
        /// <summary>
        /// Initialize player
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tex"></param>
        /// <param name="speed"></param>
        public Player(string name, Texture2D tex, int speed) : base(name, tex)
        {
            MoveSpeed = speed;
            state = PlayerState.OnLand;
            direction = Direction.Idle;
            Acceleration = new Vector2(0, 0f);
            Velocity = new Vector2(0, 0);
        }
        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Location, Color.White);
        }

        /// <summary>
        /// Main Update loop
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;
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

            //Change the velocity by acceleration
            Velocity.X += Acceleration.X;
            Velocity.Y += Acceleration.Y;

            //Then change the displacement by velocity
            Location.X += (int)Velocity.X;
            Location.Y += (int)Velocity.Y;
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
            if (InputManager.GetButtonDown("Jump") && state == PlayerState.OnLand)
            {
                state = PlayerState.IsJumping;
                Acceleration = landAcceleration;
                Velocity = jumpVelocity;
                storedJumpTime = gameTime.TotalGameTime.TotalSeconds;
            }
        }


        protected override void HandleCollisionEnter(object sender, EventArgs e)
        {
        }
        protected override void HandleCollisionExit(object sender, EventArgs e)
        {
        }
    }
}
