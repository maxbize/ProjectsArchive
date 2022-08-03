using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

/*
 * TODO:
 *  - Check OneNote
 */

namespace Zong
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ZongGame : Microsoft.Xna.Framework.Game
    {
        // Graphics vars
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
		
		// Menu system
		ScreenManager screenManager;
		static readonly string[] preloadAssets =
		{
			"gradient",
		};

        // Manages all the bricks in the level
        public LevelManager levelManager;

        // Model manager
        public ModelManager modelManager;

        public ZongGame()
        {
            Content.RootDirectory = "Content";
			
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = graphics.PreferredBackBufferWidth * 9 / 16;
			
			// Screen manager stuff
			screenManager = new ScreenManager(this);
            Components.Add(screenManager);
			screenManager.AddScreen(new BackgroundScreen(), null);
			screenManager.AddScreen(new MainMenuScreen(this), null);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        public void initSinglePlayerGame()
        {
            modelManager = new ModelManager(this);
            levelManager = new LevelManager(this, modelManager);
            modelManager.addPad(PlayerIndex.One, GraphicsDevice.Viewport);
        }

        public void initLocalMultiGame(int numPlayers)
        {
            modelManager = new ModelManager(this);
            levelManager = new LevelManager(this, modelManager);
            Viewport vp1 = GraphicsDevice.Viewport;
            Viewport vp2 = GraphicsDevice.Viewport;
            vp1.Width /= 2;
            vp2.Width = vp1.Width;
            vp2.X = vp1.Width;
            modelManager.addPad(PlayerIndex.One, vp1);
            modelManager.addPad(PlayerIndex.Two, vp2);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures such as the score.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Need to update Camera before player.
            // If this causes problems then flip player/Camera dependency
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
