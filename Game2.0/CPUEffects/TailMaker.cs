using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Game2._0.Players;
using Game2._0.Core;

namespace Game2._0.CPUEffects
{
    class TailMaker
    {
        private List<ShadedImage> ShadedImages;

        public readonly Player Owner;
        public bool Active
        {
            get;
            set;
        } = false;

        public TailMaker(Player Owner)
        {
            ShadedImages = new List<ShadedImage>();
            this.Owner = Owner;
        }

        public void Update(GameTime gameTime)
        {
            if (Active)
            {
                ShadedImages.Add(new ShadedImage() {
                    Bounds = Owner.Bounds,
                    Texture = Owner.Texture,
                    Alpha =  0.35f
                });

            }

            foreach (var shadedImage in ShadedImages)
            {
                shadedImage.Alpha -= (float)gameTime.ElapsedGameTime.TotalSeconds * 3;
            }

            ShadedImages.RemoveAll(c => c.Alpha <= 0);
        }

        public void Draw(SpriteBatch batch, Camera camera)
        {
            foreach (var shadedImage in ShadedImages)
            {
                batch.Draw(shadedImage.Texture, camera.CalculateCameraPosition(shadedImage.Bounds), Color.White * shadedImage.Alpha);
            }
        }
    }
}
