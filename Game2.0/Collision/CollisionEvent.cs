using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2._0.Collision
{
    enum CollisionSide
    {
        Vertical,
        Horizontal
    }

    class CollisionEvent
    {
        public readonly CollisionSide CollisionSide;
        public readonly ICollideable Collider;
        public readonly Vector2 CollisionAmount;

        public CollisionEvent(CollisionSide CollisionSide, ICollideable Collider, Vector2 CollisionAmount)
        {
            this.CollisionSide = CollisionSide;
            this.Collider = Collider;
            this.CollisionAmount = CollisionAmount;
        }

    }
}
