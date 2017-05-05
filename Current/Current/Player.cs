using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

namespace Current
{
    enum PlayerState
    {
        OnLand,
        InAir,
        InWater,
        IsDead,
        HasWon
    }

    enum Direction
    {
        Left,
        Right,
    }


    class Player : ComplexCollidableObject
    {

        //Player Health
        public int Health { get; set; }
        //Rate at which player will move
        public int MoveSpeed { get; set; }
        //How strong is the player??
        public int Strength { get; set; }

        public bool HasLaser { get; set; } 

        /// <summary>
        /// Reference to the laser gameobject
        /// </summary>
        public Laser LaserReference { get; set; }

        

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
        public Player(string name, Texture2D tex, Texture2D laserTex, Rectangle location) : base(name, tex, location)
        {
            //Setup various states and attributes
            MoveSpeed = 8;
            //Assume in air to begin with
            state = PlayerState.InAir;
            direction = Direction.Right;

            SpriteFX = SpriteEffects.FlipHorizontally;

            Health = startHealth;

            HasLaser = true;
            //Create the laser, and add it's animation
            LaserReference = new Laser(name + "_laser", laserTex, new Rectangle(0,0,Game1.TargetWidth, 20), this, new Point(Location.Width, Location.Height/2));
            LaserReference.AddAnimation(new Animate(laserTex, 25, 1, Animate.ONESIXTIETHSECPERFRAME, LaserReference));
            LaserReference.ChangeAnimation("Laser");



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
            if (state != PlayerState.IsDead)
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
                    CheckIfDead();
                    LaserUpdate();
                    if (Velocity == Vector2.Zero)
                        ChangeAnimation("CurrentIdle");
                    else
                        ChangeAnimation("CurrentWalk");
                    break;
                case PlayerState.InAir:
                    //Move at reduced speed
                    Move(3 * MoveSpeed / 4);
                    CheckForWaterWhenNotSwimming();
                    CheckIfDead();
                    ChangeAnimation("CurrentWalk");//Temporary
                    LaserUpdate();
                    break;
                case PlayerState.InWater:
                    Swim(MoveSpeed);
                    CheckIfDead();
                    ChangeAnimation("CurrentSwim");
                    LaserUpdate();
                    break;
                case PlayerState.IsDead:
                    //Respawn
                    if (InputManager.GetButtonDown("Jump"))
                    {
                        GameManager.RespawnAllNonUIObjects();
                        //Let everyone else resume updating.
                        GameManager.ResumeNonUIUpdates();
                    }
                    break;
                case PlayerState.HasWon:
                    break;
                default:
                    break;
            }
   

            

            base.Update(gameTime);
        }

        /// <summary>
        /// Determines if player is dead
        /// </summary>
        public void CheckIfDead()
        {           
            //If we're out of health, or below level bounds
            if (Health <= 0 || Location.Y - Location.Height > GameManager.MinLevelLocation.Y)
            {
                state = PlayerState.IsDead;
                //Freeze player
                Velocity = Vector2.Zero;
                Acceleration = Vector2.Zero;

                //Play a sound effect
                GameManager.PlaySFX("Hurt");

                //Turn off the laser
                LaserReference.Deactivate();
                GameManager.StopSFX("Laser");

                //Show a message
                GameManager.Get("GameoverText").Activate();
                GameManager.Get("GameoverInstr").Activate();
                //Stop everyone else from updating.
                GameManager.StopNonUIUpdates();
            }

        }
 
        /// <summary>
        /// Hurts the player
        /// </summary>
        public void Hurt()
        {
            Health -= 1;
            //Don't play if dead because it will play in CheckIfDead as well
            if (Health > 0)
                GameManager.PlaySFX("Hurt");
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
                //Play a sound effect
                GameManager.PlaySFX("Jump");
            }

            
        }

        /// <summary>
        /// Swim Update 
        /// </summary>
        public void Swim(int speed)
        {

            int horz = 0;
            int vert = 0;

            if (InputManager.GetButton("Right") )
            {
                horz = 1;
                SpriteFX = SpriteEffects.FlipHorizontally;
                direction = Direction.Right;
            }
            if (InputManager.GetButton("Left"))
            {
                horz = -1;
                SpriteFX = SpriteEffects.None;
                direction = Direction.Left;

            }
            if (InputManager.GetButton("Up"))
            {
                vert = 1;
            }
            if (InputManager.GetButton("Down"))
            {
                vert = -1;
            }

            if (horz != 0 || vert != 0)
            {
                Velocity = new Vector2(horz * speed, -vert * speed);
            }

  
                
            
            //Check for collisions and stop appropriate component of velocity while swimming
            if (CollLeft.CollidingWith<Platform>() && Velocity.X < 0)
                Velocity.X = 0;
            if (CollRight.CollidingWith<Platform>() && Velocity.X > 0)
                Velocity.X = 0;
            if (CollAbove.CollidingWith<Platform>() && Velocity.Y < 0) 
                Velocity.Y = 0;
            if (CollBelow.CollidingWith<Platform>() && Velocity.Y > 0)
                Velocity.Y = 0;


            //Exit swimming state when not colliding with any water objects
            //Done here rather than in HandleCollision events because there 
            //will be a lot of water tiles near each other.
            if (!Coll.CollidingWith<Water>())
            {
                state = PlayerState.InAir;
                Acceleration = airAcceleration;

                if (Velocity.Y <= 0)
                    Velocity.Y = .5f * jumpVelocity.Y;

                //Turn back to normal color
                DrawColor = Color.White;

                //Stop ambient swim noise
                GameManager.StopSFX("WaterLoop");
            }
        }

        /// <summary>
        /// Laser update logic
        /// </summary>
        public void LaserUpdate()
        {

            if (!HasLaser)
                return;

            if (InputManager.GetButtonDown("Fire"))
            {
                LaserReference.Activate();
                GameManager.LoopSFX("Laser");
            }
            if (InputManager.GetButtonUp("Fire"))
            {
                LaserReference.Deactivate();
                GameManager.StopSFX("Laser");
            }


            if (direction == Direction.Right)
                LaserReference.Offset = new Point(3*Location.Width/4, Location.Height / 2);
            else
                LaserReference.Offset = new Point(-LaserReference.Location.Width + Location.Width/4, Location.Height / 2);

        }

        /// <summary>
        /// Check for a collision with water when we're not swimming
        /// </summary>
        /// <param name="other">Collider to check with</param>
        private void CheckForWaterWhenNotSwimming()
        {
            if (Coll.CollidingWith<Water>() && !CollBelow.CollidingWith<Platform>())
            {
                state = PlayerState.InWater;
                Acceleration = .05f * airAcceleration;
                Velocity.Y = MathHelper.Clamp(Velocity.Y, -MoveSpeed, MoveSpeed);

                //Turn blue to match water
                DrawColor = Color.Blue;

                //Loop ambient swim noise
                GameManager.LoopSFX("WaterLoop");
            }

        }

        /// <summary>
        /// Set the health to the initial health, and respawn
        /// </summary>
        public override void Reset()
        {
            Respawn();
            Health = startHealth;
            base.Reset();
        }
        /// <summary>
        /// Set health to initial health, and move to location of last checkpoint.
        /// </summary>
        public override void Respawn()
        {
            state = PlayerState.InAir;
            Acceleration = airAcceleration;

            //Reset color
            DrawColor = Color.White;

            //Hide death message
            GameManager.Get("GameoverText").Deactivate();
            GameManager.Get("GameoverInstr").Deactivate();

            //Stop ambient swim noise if we die in the water
            GameManager.StopSFX("WaterLoop");

            Health = startHealth;
            base.Respawn();
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
                        if (!CollBelow.CollidingWith<Platform>() && !Coll.CollidingWith<Water>())
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
