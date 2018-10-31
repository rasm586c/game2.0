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
using Game2._0.Collision;
using Game2._0.Abilities;
using Game2._0.Abilities.Uncategorized;

namespace Game2._0
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Player player;
        private Camera camera;

        private CollisionHandler collisionHandler;

        List<IGameObject> blocks;
        List<IGameObject> objectsOnScreen;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadBlocks();

            camera = new Camera(Window.ClientBounds.Width, Window.ClientBounds.Height);

            collisionHandler = new CollisionHandler();
            player = new Player(Content, collisionHandler);
            player.LoadContent();

            camera.Focus(player.Bounds);



            player.Abilities = new IAbility[] {
                new Dash(player, collisionHandler)
            };




        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            FindObjectsOnScreen();
            collisionHandler.UpdateScreenCollideables(objectsOnScreen.Cast<ICollideable>());

            player.Update(gameTime);

            camera.SoftFocus(player.Bounds, 0.05f, 0);

            // if player.velocity > 50 camera.Focus( player.Bounds ),


            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                player.Abilities[0].Invoke();
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue); // clear backbuffer
            spriteBatch.Begin();

            DrawBlocks(objectsOnScreen);
            player.Draw(spriteBatch, camera);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void LoadBlocks()
        {
            blocks = new List<IGameObject>();

            for (int i = 0; i < 10000; i++)
            {
                var block = new BlockObject(Content);
                block.LoadContent();
                block.SetPosition(i * block.Bounds.Width, 0);
                blocks.Add(block);
            }

            for (int i = 0; i < 30; i++)
            {
                var block = new BlockObject(Content);
                block.LoadContent();
                block.SetPosition(500 - i * block.Bounds.Width, block.Bounds.Height);
                blocks.Add(block);
            }

            for (int i = -10000; i < -10; i++)
            {
                var block = new BlockObject(Content);
                block.LoadContent();
                block.SetPosition(i * block.Bounds.Width, 0);
                blocks.Add(block);
            }


            for (int i = 0; i < 25; i++)
            {
                var block = new BlockObject(Content);
                block.LoadContent();
                block.SetPosition(200 + (i * block.Bounds.Width), -block.Bounds.Height);
                blocks.Add(block);
            }

            blocks = blocks.OrderBy(v => v.Bounds.X).ToList();
        }

        private void DrawBlocks(List<IGameObject> blocks)
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].Draw(spriteBatch, camera);
            }
        }
        private void FindObjectsOnScreen()
        {
            objectsOnScreen = camera.GetObjectsOnScreen(blocks);
        }
    }
}
