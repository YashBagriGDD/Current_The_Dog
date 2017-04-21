﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Current
{
    enum EnemyState {
        Roaming,
        PlayerDetected,
        Returning
    }

    class Catfish : Enemy
    {
        public EnemyState state { get; set; }
        public Direction direction { get; set; }
        public bool IsAlive { get; set; }

        //Max speed the Catfish goes at
        const float MAX_SPEED = 15f;
        //How far until enemy notices player
        const int MAX_DETECTION = 200;

        //When enemy is wandering, keep track of how many seconds has passed. Change direction after set amount of time
        public double TotalElapsedSeconds { get; set; } = 0;
        public object Thread { get; private set; }

        const double MoveChangeTime = 2.0; //seconds

        public Catfish(string name, Texture2D tex, Rectangle location) : base(name, tex, location)
        {
            state = EnemyState.Roaming;
            direction = Direction.Left;
            Health = 1;
            IsAlive = true;
            SetSpeed();
            HomePoint = new Vector2(Location.X, Location.Y);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Only draw enemy if it is alive
            if (IsAlive == true) {
                base.Draw(gameTime, spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            //add update logic here
            switch (state) {
                case EnemyState.Roaming:
                    //Check to see if player is within range of detection
                    GameObject player = GameManager.Get("Current");

                    //store the distance between
                    int distX = player.Location.X - this.Location.X;
                    int distY = player.Location.Y - this.Location.Y;

                    if (Math.Abs(distX) <= MAX_DETECTION || Math.Abs(distY) <= MAX_DETECTION) {
                        state = EnemyState.PlayerDetected;
                        break;
                    }

                    //Call wander method to move enemy
                    Wander(gameTime);
                    break;
                case EnemyState.PlayerDetected:
                    //Call Chase player method
                    ChasePlayer();

                    //Once method is broken, go to return enum
                    state = EnemyState.Returning;
                    break;
                case EnemyState.Returning:
                    //Call ReturnHome Method
                    ReturnHome();
                    //Once it ends go into Roaming
                    state = EnemyState.Roaming;
                    break;
                default:
                    break;
            }

            //Death logic
            if (Health <= 0) {
                IsAlive = false;
            }

            base.Update(gameTime);
        }

        protected override void HandleCollisionEnter(object sender, EventArgs e)
        {
            //Cast to a Collider
            Collider other = (Collider)sender;

            //Player Collision
            if (other.Host is Player) {
                //Move enemy after collision
                Location.X += 5;
                Location.Y += 5;

                //Stop enemy for a time
                System.Threading.Thread.Sleep(1500);
            }

            //Coral Collision
            if (other.Host is Coral) {
                Health--;
                Location.X -= (Location.X - other.Hitbox.Y);
                Location.Y -= (Location.Y - other.Hitbox.Y);
            }
        }

        protected override void HandleCollisionExit(object sender, EventArgs e)
        {
            //Cast to a Collider
            Collider other = (Collider)sender;

            //Set to return home after colliding with Coral
            if (other.Host is Coral) {
                state = EnemyState.Returning;
            }
        }

        /// <summary>
        /// Has enemy move back and forth in a time interval
        /// </summary>
        /// <param name="gameTime"></param>
        public void Wander(GameTime gameTime) {
            TotalElapsedSeconds += gameTime.ElapsedGameTime.TotalSeconds;

            if (TotalElapsedSeconds >= MoveChangeTime) {
                TotalElapsedSeconds -= MoveChangeTime;
                ChangeDirection();
                SetSpeed();
            }

            this.Location.X += (int)Speed;
        }

        /// <summary>
        /// Switched direction from left to right, or right to left
        /// </summary>
        public void ChangeDirection() {
            switch (direction) {
                case Direction.Left:
                    //Change  direction
                    direction = Direction.Right;
                    break;
                case Direction.Right:
                    //Change Direction
                    direction = Direction.Left;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Sets the speed of the enemy depending on if it is Chasing the player or not and which direction
        /// </summary>
        public void SetSpeed() {
            switch (direction) {
                case Direction.Left:
                    //sets speed for new direction depending on state
                    //If Player is detected, will go to max speed otherwise will move slower
                    if (state == EnemyState.PlayerDetected) {
                        Speed = MAX_SPEED;
                    }
                    else {
                        Speed = (float)(.25 * MAX_SPEED);
                    }
                    break;
                case Direction.Right:
                    //sets speed for new direction depending on state
                    if (state == EnemyState.PlayerDetected) {
                        Speed = -1 * MAX_SPEED;
                    }
                    else {
                        Speed = (float)(-.25 * MAX_SPEED);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Finds the distance between enemy and player and advances toward player
        /// Once player exits range method ends
        /// </summary>
        public void ChasePlayer() {
            GameObject player = GameManager.Get("Current");

            //store the distance between
            int distX = player.Location.X - this.Location.X;
            int distY = player.Location.Y - this.Location.Y;

            //Check to see if player is within range
            //This checks to see if player is within a 200x200 square around the enemy
            while (Math.Abs(distX) <= MAX_DETECTION || Math.Abs(distY) <= MAX_DETECTION) {
                //Check if to the left or right of the player, set speed to player direction and move
                if (distX < 0) {
                    direction = Direction.Left;
                    SetSpeed();
                }
                if (distX > 0) {
                    direction = Direction.Right;
                    SetSpeed();
                }
                Location.X += (int)Speed;

                //Check if below or above player, then move it in player direction
                if (distY < 0) {
                    Location.Y -= (int)Math.Abs(Speed) * -1;
                }
                if (distY > 0) {
                    Location.Y -= (int)Math.Abs(Speed);
                }

                //Update distance between this and player
                distX = player.Location.X - this.Location.X;
                distY = player.Location.Y - this.Location.Y;
            }
        }

        /// <summary>
        /// Makes the enemy return to it's homepoint. Once within a certain range of the home, exits method
        /// </summary>
        public void ReturnHome() {
            //find distance between enemy and home
            int distX = Location.X - (int)HomePoint.X;
            int distY = Location.Y - (int)HomePoint.Y;

            //loop to it going back to home
            while (Math.Abs(distX) > 5 || Math.Abs(distY) > 5) {
                //X Movement
                if (distX < 0) {
                    direction = Direction.Right;
                    SetSpeed();
                }
                if (distX > 0) {
                    direction = Direction.Left;
                    SetSpeed();
                }
                Location.X += (int)Speed;

                //y direction
                if (distY > 0)
                    Location.Y += -1 * (int)Math.Abs(Speed);
                else
                    Location.Y += (int)Math.Abs(Speed);

                //Update distances
                distX = Location.X - (int)HomePoint.X;
                distY = Location.Y - (int)HomePoint.Y;
            }
        }
    }
}
