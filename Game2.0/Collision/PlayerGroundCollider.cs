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


namespace Game2._0.Collision
{
    class PlayerGroundCollider : ICollideable
    {
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(player.Bounds.Left, player.Bounds.Bottom + 1, player.Bounds.Width, 1);
            }
        }

        private readonly Player player;

        public PlayerGroundCollider(Player player)
        {
            this.player = player;
        }


        public bool CollidesWith(ICollideable Collider)
        {
            return Bounds.Intersects(Collider.Bounds);
        }
    }
}
