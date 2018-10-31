using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game2._0.Collision;

namespace Game2._0.Core
{
    interface IGameObject : ICollideable
    {
        void Draw(SpriteBatch batch, Camera camera);

        void Update(GameTime time);
    }
}
