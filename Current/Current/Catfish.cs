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

    class Catfish : Enemy
    {
        public EnemyState state { get; set; }
        public bool IsAlive { get; set; }

        public Catfish(string name, Texture2D tex, Rectangle location) : base(name, tex, location)
        {
            state = EnemyState.Roaming;
            Health = 1;
            IsAlive = true;
            Speed = 3;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsAlive == true) {
                base.Draw(gameTime, spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            //add update logic here
            switch (state) {
                case EnemyState.Roaming:
                    break;
                case EnemyState.PlayerDetected:
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
    }
}
