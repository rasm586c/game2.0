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
using Game2._0.Abilities;

namespace Game2._0.Players
{
    class Player : ICollideable, IGameObject, IAbilityCaster
    {
        /* Properties */
        public Rectangle Bounds
        {
            get;
            private set;
        }

        public Point Position
        {
            get
            {
                return new Point(Bounds.X, Bounds.Y);
            }

            set
            {
                Bounds = new Rectangle(value.X, value.Y, Bounds.Width, Bounds.Height);
            }
        }

        public bool IsOnGround
        {
            get
            {
                return isOnGround;
            }
        }
        bool isOnGround;
        
        public Direction FacingDirection
        {
            get;
            private set;
        }

        public IAbility[] Abilities
        {
            get;
            set;
        }

        public Texture2D Texture
        {
            get
            {
                return texture;
            }
        }

        /* Fields */
        private Texture2D texture;
        private ContentManager content;
        private CollisionHandler collisionHandler;

        private float movement;
        Vector2 velocity;

        // Constants for controling horizontal movement
        private const float MoveAcceleration = 13000.0f;
        private const float MaxMoveSpeed = 1750.0f;
        private const float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.58f;

        // Constants for controlling vertical movement
        private const float MaxJumpTime = 0.35f;
        private const float JumpLaunchVelocity = -3500.0f;
        private const float GravityAcceleration = 3400.0f;
        private const float MaxFallSpeed = 550.0f;
        private const float JumpControlPower = 0.14f;

        // Jumping state
        private bool isJumping;
        private bool wasJumping;
        private float jumpTime;

        /* Constructor(s) */
        public Player(ContentManager content, CollisionHandler collisionHandler)
        {
            this.content = content;
            this.collisionHandler = collisionHandler;
        }

        /* Methods */
        public void LoadContent()
        {
            texture = content.Load<Texture2D>("Player/SpriteTest");
            Bounds = new Rectangle(-400, 0, texture.Width, texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            if (camera.InteractsWith(this))
            {
                spriteBatch.Draw(texture, camera.CalculateCameraPosition(Bounds).ToVector2(), null, Color.White, 0.0f, Vector2.Zero, 1.0f, FacingDirection == Direction.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

                foreach (IAbility ability in Abilities)
                {
                    ability.Draw(spriteBatch, camera);
                }
            }
        }

        public void Update(GameTime time)
        {
            KeyboardState ks = Keyboard.GetState();
            
            GetMovement(ks);
            ApplyPhysics(time);
            UpdateAbilities(time);

            // Clear input.
            if (movement > 0)
                FacingDirection = Direction.Right;
            else if (movement < 0)
                FacingDirection = Direction.Left;

            movement = 0.0f;
        }


        public bool CollidesWith(ICollideable Collider)
        {
            return Bounds.Intersects(Collider.Bounds);
        }

        private void UpdateAbilities(GameTime time)
        {
            foreach (IAbility ability in Abilities)
            {
                ability.Update(time);
            }
        }

        private void GetMovement(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Left) ||
                keyboardState.IsKeyDown(Keys.A))
            {
                movement = -1.0f;
            }
            else if (keyboardState.IsKeyDown(Keys.Right) ||
                     keyboardState.IsKeyDown(Keys.D))
            {
                movement = 1.0f;
            }

            // Check if the player wants to jump.
            isJumping = keyboardState.IsKeyDown(Keys.Space) ||
                keyboardState.IsKeyDown(Keys.Up) ||
                keyboardState.IsKeyDown(Keys.W);
        }

        private float DoJump(float velocityY, GameTime gameTime)
        {
            // If the player wants to jump
            if (isJumping)
            {
                // Begin or continue a jump
                if ((!wasJumping && IsOnGround) || jumpTime > 0.0f)
                {
                    jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    // Jump Animation!
                }

                // If we are in the ascent of the jump
                if (0.0f < jumpTime && jumpTime <= MaxJumpTime)
                {
                    // Fully override the vertical velocity with a power curve that gives players more control over the top of the jump
                    velocityY = JumpLaunchVelocity * (1.0f - (float)Math.Pow(jumpTime / MaxJumpTime, JumpControlPower));
                }
                else
                {
                    // Reached the apex of the jump
                    jumpTime = 0.0f;
                }
            }
            else
            {
                // Continues not jumping or cancels a jump in progress
                jumpTime = 0.0f;
            }
            wasJumping = isJumping;

            return velocityY;
        }

        private void ApplyPhysics(GameTime time)
        {
            float elapsed = (float)time.ElapsedGameTime.TotalSeconds;
            Vector2 lastPosition = new Vector2(Position.X, Position.Y);

            // Base velocity is a combination of horizontal movement control and
            // acceleration downward due to gravity.
            velocity.X += movement * MoveAcceleration * elapsed;
            velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * elapsed, -MaxFallSpeed, MaxFallSpeed);

            velocity.Y = DoJump(velocity.Y, time);

            // Apply pseudo-drag horizontally.
            if (IsOnGround)
                velocity.X *= GroundDragFactor;
            else
                velocity.X *= AirDragFactor;

            // Apply velocity.
            Move(velocity * elapsed);

            // If the player is now colliding with the level, separate them.
            HandleCollisions();

            // If the collision stopped us from moving, reset the velocity to zero.
            if (Position.X == lastPosition.X)
                velocity.X = 0;

            if (Position.Y == lastPosition.Y)
                velocity.Y = 0;
        }
        private void HandleCollisions()
        {
            List<CollisionEvent> collisionEvents;

            isOnGround = false;

            while ((collisionEvents = collisionHandler.GetCollisions(this)).Count > 0)
            {
                CollisionEvent collisionEvent = collisionEvents.First();

                if (collisionEvent.CollisionSide == CollisionSide.Horizontal)
                {
                    Move((int)-collisionEvent.CollisionAmount.X, 0);
                }
                else if (collisionEvent.CollisionSide == CollisionSide.Vertical)
                {
                    Move(0, -(int)collisionEvent.CollisionAmount.Y);
                    isOnGround = true;
                }
            }

            if (!isOnGround)
            {
                isOnGround = collisionHandler.GetCollisions(new PlayerGroundCollider(this)).Count > 0;
            }
        }

        private void Move(int dx, int dy)
        {
            Position = new Point(Position.X + dx, Position.Y + dy);
            
        }
        private void Move(Vector2 delta)
        {
            Move((int)Math.Round(delta.X), (int)Math.Round(delta.Y));
        }
    }
}
