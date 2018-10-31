using Game2._0.Collision;
using Game2._0.Core;
using Game2._0.CPUEffects;
using Game2._0.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2._0.Abilities.Uncategorized
{
    class Dash : IAbility
    {
        /* Properties */
        public string Name
        {
            get;
            private set;
        } = "Dash";

        public DateTime LastActivated
        {
            get;
            private set;
        }

        public TimeSpan Cooldown
        {
            get;
            private set;
        } = TimeSpan.FromSeconds(3);

        public bool OnCooldown
        {
            get
            {
                return DateTime.UtcNow < LastActivated.Add(Cooldown);
            }
        }

        /* Fields */
        private const int DashDistance = 300;
        private const int DashSpeed = 35;

        private Player owner;
        private Vector2 startPosition;
        private bool active;
        private int forward;

        private CollisionHandler collisionHandler;

        private TailMaker tailMaker;

        /* Constructor(s) */
        public Dash(Player owner, CollisionHandler collisionHandler)
        {
            LastActivated = DateTime.UtcNow.Subtract(Cooldown);

            this.collisionHandler = collisionHandler;
            this.owner = owner;

            tailMaker = new TailMaker(owner);
        }

        /* Methods */
        public void Invoke()
        {
            if (!OnCooldown)
            {
                ActivateCooldown();
                InitializeAbility();
                SaveForward();
            }
        }
        
        public void Update(GameTime time) {
            if (active)
            {
                MoveOwnerForward(DashSpeed);
                StopConditions();
            }
            
            tailMaker.Active = active;
            tailMaker.Update(time);
        }

        public void Draw(SpriteBatch batch, Camera camera) {
            tailMaker.Draw(batch, camera);
        }

        private void MoveOwnerForward(int amount)
        {
            owner.Position = new Point(owner.Position.X + amount * forward, owner.Position.Y);
        }

        private void SaveForward()
        {
            forward = owner.FacingDirection == Core.Direction.Right ? 1 : -1;
        }

        private void ActivateCooldown()
        {
            LastActivated = DateTime.UtcNow;
        }

        private void InitializeAbility()
        {
            active = true;
            startPosition = owner.Position.ToVector2();
        }

        private void StopConditions()
        {
            var collisions = collisionHandler.GetCollisions(owner);

            if (collisions.Any(c => c.CollisionSide == CollisionSide.Horizontal) || Vector2.Distance(startPosition, owner.Position.ToVector2()) > DashDistance)
                active = false;
        }
    }
}
