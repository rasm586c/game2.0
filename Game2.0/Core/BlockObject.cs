using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game2._0.Collision;
using Game2._0.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2._0.Core
{
    class BlockObject : IGameObject, ICollideable
    {
        /* Properties */
        public Rectangle Bounds
        {
            get;
            private set;
        }

        /* Fields */
        private Texture2D texture;

        private ContentManager content;

        private Point position;

        /* Constructor(s) */
        public BlockObject(ContentManager content)
        {
            this.content = content;
            position = new Point(0, 0);
        }

        public BlockObject(ContentManager content, int x, int y) : this(content)
        {
            position = new Point(x, y);
        }



        /* Methods */

        public void LoadContent()
        {
            texture = content.Load<Texture2D>("Player/BlackBlock");
            Bounds = new Rectangle(position.X, position.Y, texture.Width, texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            if (camera.InteractsWith(this))
            {
                spriteBatch.Draw(texture, camera.CalculateCameraPosition(Bounds), Color.White);
            }
        }

        public void Update(GameTime time) { }

        public bool CollidesWith(ICollideable Collider)
        {
            return Bounds.Intersects(Collider.Bounds);
        }

        public void SetPosition(int x, int y)
        {
            position = new Point(x, y);
            Bounds = new Rectangle(position.X, position.Y, Bounds.Width, Bounds.Height);
        }

        private void Move(int dx, int dy)
        {
            Bounds = new Rectangle(Bounds.X + dx, Bounds.Y + dy, Bounds.Width, Bounds.Height);
        }
    }
}
