using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        const double MoveChangeTime = 2.0; //seconds

        public Catfish(string name, Texture2D tex, Rectangle location) : base(name, tex, location)
        {
            state = EnemyState.Roaming;
            direction = Direction.Left;
            Health = 1;
            IsAlive = true;
            Speed = (float)(-.25 * MAX_SPEED);
            
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

                    break;
                default:
                    break;
            }

            base.Update(gameTime);
        }

        protected override void HandleCollisionEnter(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void HandleCollisionExit(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Wander(GameTime gameTime) {
            TotalElapsedSeconds += gameTime.ElapsedGameTime.TotalSeconds;

            if (TotalElapsedSeconds >= MoveChangeTime) {
                TotalElapsedSeconds -= MoveChangeTime;
                ChangeDirection();
                SetSpeed();
            }

            this.Location.X += (int)Speed;
        }

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

        public void SetSpeed() {
            switch (direction) {
                case Direction.Left:
                    //sets speed for new direction depending on state
                    if (state == EnemyState.Roaming) {
                        Speed = (float)(.25 * MAX_SPEED);
                    }
                    else {
                        Speed = MAX_SPEED;
                    }
                    break;
                case Direction.Right:
                    //sets speed for new direction depending on state
                    if (state == EnemyState.Roaming) {
                        Speed = (float)(-.25 * MAX_SPEED);
                    }
                    else {
                        Speed = -1 * MAX_SPEED;
                    }
                    break;
                default:
                    break;
            }
        }

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

        public void ReturnHome() {

        }
    }
}
