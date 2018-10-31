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
    class CollisionHandler
    {
        private IEnumerable<ICollideable> collideables;
        public CollisionHandler()
        {
            collideables = new List<ICollideable>();
        }

        public List<CollisionEvent> GetCollisions(ICollideable player)
        {
            List<CollisionEvent> collisions = new List<CollisionEvent>();
            foreach (ICollideable stationary in collideables)
            {
                if (stationary.CollidesWith(player))
                {
                    Vector2 depth = stationary.Bounds.GetIntersectionDepth(player.Bounds);

                    float absDepthX = Math.Abs(depth.X);
                    float absDepthY = Math.Abs(depth.Y);
                    
                    collisions.Add(new CollisionEvent(absDepthY < absDepthX ? CollisionSide.Vertical : CollisionSide.Horizontal, stationary, depth));
                }
            }

            return collisions;
        }

        public void UpdateScreenCollideables(IEnumerable<ICollideable> collideables)
        {
            this.collideables = collideables;
        }
    }
}
