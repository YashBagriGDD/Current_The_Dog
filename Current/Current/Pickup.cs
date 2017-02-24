using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Current
{
    /// <summary>
    /// Abstract class representing an object that can be picked up
    /// </summary>
    abstract class Pickup : CollidableObject
    {
        public int Value { get; set; }

        public Pickup(string name, Texture2D texture) : base(name, texture)
        {
        }
    }
}
