using Game2._0.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Game2._0.Abilities
{
    interface IAbility
    {
        string Name
        {
            get;
        }

        DateTime LastActivated
        {
            get;
        }
        
        TimeSpan Cooldown
        {
            get;
        }

        void Invoke();

        void Update(GameTime time);

        void Draw(SpriteBatch batch, Camera camera);
    }
}
