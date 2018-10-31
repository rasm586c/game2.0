using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Game2._0.CPUEffects
{
    class ShadedImage
    {
        public Rectangle Bounds
        {
            get;
            set;
        }

        public Texture2D Texture
        {
            get;
            set;
        }

        public float Alpha
        {
            get;
            set;
        }
    }
}
