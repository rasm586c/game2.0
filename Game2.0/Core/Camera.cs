using Game2._0.Collision;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game2._0.Core
{
    class Camera
    {
        public Vector2 Position
        {
            get;
            set;
        } = new Vector2(0, 0);

        private Point ScreenDimensions;
        private int ScreenMargin = 800;

        public Camera(int ScreenWidth, int ScreenHeight)
        {
            ScreenDimensions = new Point(ScreenWidth, ScreenHeight);
        }

        public void Focus(Rectangle Bounds)
        {
            int xPos = Bounds.X - ((ScreenDimensions.X - Bounds.Width) / 2);
            int yPos = Bounds.Y - ((ScreenDimensions.Y - Bounds.Height) / 2);
            Position = new Vector2(xPos, yPos);
        }

        public void SoftFocus(Rectangle Bounds, float strength = 0.01f, float jiggerTolerance = 25)
        {
            int targetPosX = Bounds.X - ((ScreenDimensions.X - Bounds.Width) / 2);
            int targetPosY = Bounds.Y - ((ScreenDimensions.Y - Bounds.Height) / 2);

            var Target = new Vector2(targetPosX, targetPosY);
            var TargetDistance = Vector2.Distance(Position, Target);


            if (TargetDistance > jiggerTolerance)
                Position = Vector2.Lerp(Position, Target, strength);
            if (TargetDistance > ScreenDimensions.X)
                Focus(Bounds);
        }

        public void LeftFocus(Rectangle Bounds)
        {
            int yPos = Bounds.Y - ((ScreenDimensions.Y - Bounds.Height) / 2);
            Position = new Vector2(Bounds.X, yPos);
        }

        public void RightFocus(Rectangle Bounds)
        {
            int yPos = Bounds.Y - ((ScreenDimensions.Y - Bounds.Height) / 2);
            Position = new Vector2(Bounds.X - ScreenDimensions.X , yPos);
        }


        public List<IGameObject> GetObjectsOnScreen(List<IGameObject> blocks)
        {
            List<IGameObject> screenObjects = new List<IGameObject>();
            foreach (var block in blocks)
            {
                if (InteractsWith(block))
                {
                    screenObjects.Add(block);
                }
                else
                {
                    if (block.Bounds.X > Position.X + ScreenDimensions.X + ScreenMargin / 2)
                    {
                        break;
                    }
                }
            }
            return screenObjects;
        }

        public Rectangle CalculateCameraPosition(Rectangle Bounds)
        {
            return new Rectangle(Bounds.X - (int)Position.X, Bounds.Y - (int)Position.Y, Bounds.Width, Bounds.Height);
        }

        public bool InteractsWith(Rectangle bounds)
        {
            Rectangle Screen = new Rectangle((int)(Position.X - ScreenMargin / 2), (int)(Position.Y - ScreenMargin / 2), ScreenDimensions.X + ScreenMargin, ScreenDimensions.Y + ScreenMargin);
            return Screen.Intersects(bounds);
        }

        public bool InteractsWith(ICollideable collider)
        {
            Rectangle Screen = new Rectangle((int)(Position.X - ScreenMargin / 2), (int)(Position.Y - ScreenMargin / 2), ScreenDimensions.X + ScreenMargin, ScreenDimensions.Y + ScreenMargin);
            return Screen.Intersects(collider.Bounds);
        }

    }
}
