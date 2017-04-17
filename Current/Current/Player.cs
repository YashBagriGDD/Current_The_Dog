﻿using System;
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
        Left,
        Right
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
        public bool IsAlive { get; set; }
        //The current time
        private GameTime gameTime;


        //The initial velocity for jumping
        private Vector2 jumpVelocity = new Vector2(0, -25);
        //The acceleration used in the air
        private Vector2 airAcceleration = new Vector2(0, 1);


        private int startHealth = 3;
        
        /// <summary>
        /// Initialize player
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tex"></param>
        /// <param name="speed"></param>
        public Player(string name, Texture2D tex, Rectangle location) : base(name, tex, location)
        {
            //Setup various states and attributes
            MoveSpeed = 8;
            //Assume in air to begin with
            state = PlayerState.InAir;
            direction = Direction.Right;

            SpriteFX = SpriteEffects.FlipHorizontally;

            Health = startHealth;

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
                    CheckForWaterWhenNotSwimming();
                    break;
                case PlayerState.InAir:
                    //Move at reduced speed
                    Move(3*MoveSpeed/4);
                    Jump();
                    CheckForWaterWhenNotSwimming();
                    break;
                case PlayerState.InWater:
                    Swim(MoveSpeed);
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
                SpriteFX = SpriteEffects.FlipHorizontally;
                direction = Direction.Right;
            }
            else if (InputManager.GetButton("Left") && !CollLeft.CollidingWith<Platform>())
            {
                Velocity.X = -speed;
                SpriteFX = SpriteEffects.None;
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
        /// Swim Update 
        /// </summary>
        public void Swim(int speed)
        {
            if (InputManager.GetButtonDown("Right") )
            {
                Velocity = new Vector2(speed, 0);
                SpriteFX = SpriteEffects.FlipHorizontally;
                direction = Direction.Right;
            }
            if (InputManager.GetButtonDown("Left"))
            {
                Velocity = new Vector2(-speed, 0);
                SpriteFX = SpriteEffects.None;
                direction = Direction.Left;
            }
            if (InputManager.GetButtonDown("Up"))
            {
                Velocity = new Vector2(0, -speed);
            }
            if (InputManager.GetButtonDown("Down"))
            {
                Velocity = new Vector2(0, speed);
            }

            //Check for collisions and stop appropriate component of velocity while swimming
            if (CollLeft.CollidingWith<Platform>() && Velocity.X < 0)
                Velocity.X = 0;
            if (CollRight.CollidingWith<Platform>() && Velocity.X > 0)
                Velocity.X = 0;
            if (CollAbove.CollidingWith<Platform>() && !CollLeft.CollidingWith<Water>() && !CollRight.CollidingWith<Water>() && Velocity.Y < 0)
                Velocity.Y = 0;
            if (CollBelow.CollidingWith<Platform>() && !CollLeft.CollidingWith<Water>() && !CollRight.CollidingWith<Water>() && Velocity.Y < 0)
                Velocity.Y = 0;


            //Exit swimming state when not colliding with any water objects
            //Done here rather than in HandleCollision events because there 
            //will be a lot of water tiles near each other.
            if (CollBelow.CollidingWith<Platform>() || !Coll.CollidingWith<Water>())
            {
                state = PlayerState.InAir;
                Acceleration = airAcceleration;
                if (Velocity.Y < 0)
                    Velocity = .5f * jumpVelocity;
            }
        }

        /// <summary>
        /// Check for a collision with water when we're not swimming
        /// </summary>
        /// <param name="other">Collider to check with</param>
        private void CheckForWaterWhenNotSwimming()
        {
            if (!CollBelow.CollidingWith<Platform>() && Coll.CollidingWith<Water>() )
            {
                state = PlayerState.InWater;
                Velocity = Vector2.Normalize(Velocity) * MoveSpeed;
                Acceleration = .05f * airAcceleration;
            }

        }


        public override void Reset()
        {
            Health = startHealth;
            base.Reset();

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
                   // CheckForWaterWhenNotSwimming(other);
                    break;
                case PlayerState.InAir:

                    //Stop on a platform
                    if (other.Host is Platform)
                    {
                        //When colliding below, stop
                        if (CollBelow.CollidingWith(other.Host) )
                        {
                            Acceleration = Vector2.Zero;
                            Velocity.Y = 0;
                            state = PlayerState.OnLand;
                            //Snap to the top of the platform we just landed on
                            Location.Y = (int)(other.Host.Location.Y- (.95f*Location.Height));
                        }
                        //When colliding above, reset y velocity
                        else if (CollAbove.CollidingWith(other.Host))
                        {
                            Velocity.Y = 0;
                        }
                        
                    }
                    //CheckForWaterWhenNotSwimming(other);
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
                    //If you walk off a platform, you should fall if you're no longer colliding below with anything
                    if (other.Host is Platform)
                    {
                        if (!CollBelow.CollidingWith<Platform>() && !CollBelow.CollidingWith<Water>())
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
