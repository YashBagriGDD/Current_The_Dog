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
        PlayerDetected
    }

    enum Direction {
        Left,
        Right
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

                    //Set speed based on current direction
                    switch (direction) {
                        case Direction.Left:
                            Speed = (float)(-.25 * MAX_SPEED);
                    break;
                        case Direction.Right:
                            Speed = (float)(.25 * MAX_SPEED);
                            break;
                        default:
                            break;
                    }

                    //Call wander method to move enemy
                    Wander(gameTime);
                    break;
                case EnemyState.PlayerDetected:
                    //Check to see if player is still in range of detection

                    //Set speed based on current direction
                    switch (direction) {
                        case Direction.Left:
                            Speed = (float)(-1 * MAX_SPEED);
                            break;
                        case Direction.Right:
                            Speed = (float)(MAX_SPEED);
                            break;
                        default:
                            break;
                    }

                    //Move Catfish

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
            }

            this.Location.X += (int)Speed;
        }

        public void ChangeDirection() {
            switch (direction) {
                case Direction.Left:
                    direction = Direction.Right;
                    break;
                case Direction.Right:
                    direction = Direction.Left;
                    break;
                default:
                    break;
            }
        }
    }
}
