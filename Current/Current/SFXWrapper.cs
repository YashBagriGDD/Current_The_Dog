using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Current
{
    /// <summary>
    /// Simple class to handle the playing of a specific sound effect
    /// </summary>
    class SFXWrapper : GameObject
    {

        /// <summary>
        /// The inner soundeffect
        /// </summary>
        private SoundEffect sfx;

        /// <summary>
        /// The inner soundeffect instance used for looping
        /// </summary>
        private SoundEffectInstance instance;

        public SFXWrapper(string name, SoundEffect sfx) : base(name, Rectangle.Empty)
        {
            //Make an instance of the sound effect so we can control stop and start it at will.
            this.sfx = sfx;
        }

        /// <summary>
        /// Plays the sound effect
        /// </summary>
        public void Play()
        {
            if (instance == null)
                sfx.Play();
            else
                instance.Play();
        }

        /// <summary>
        /// Loop the sound effect
        /// </summary>
        public void Loop()
        {
            if (instance == null)
                instance = sfx.CreateInstance();
            instance.IsLooped = true;
            instance.Play();
        }

        /// <summary>
        /// Stop playing the sound effect
        /// </summary>
        public void Stop()
        {
            if (instance != null)
                instance.Stop();
        }
    }
}
