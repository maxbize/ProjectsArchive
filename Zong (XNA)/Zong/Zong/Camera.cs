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


namespace Zong
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Camera : Microsoft.Xna.Framework.GameComponent
    {
        // Camera matrices
        public Matrix view { get; protected set; }
        public Matrix projection { get; protected set; }

        // Aim the camera at the CG of the level
        public Vector3 focus { get; protected set; }
        public Vector3 targetFocus { get; set; }

        // Store the camera movement about center here and add focus translation after
        Matrix subWorld;

        // Camera movement speed
        float speed = 0.16f;

        // Player that's controlling the camera
        PlayerIndex player;
        
        // For split-screen
        public Viewport viewport { get; set; }

        public Matrix world { get; protected set; }

        public Camera(Game game, PlayerIndex myPlayer, Viewport myViewport)
            : base(game)
        {
            focus = Vector3.Zero;
            targetFocus = Vector3.Zero;
            player = myPlayer;

            float theta = Functions.nextRandom(0, 4) * 90 / 180.0f * MathHelper.Pi;
            float phi = Functions.nextRandom(0, 4) * 90 / 180.0f * MathHelper.Pi;
            world = Matrix.Identity;
            world *= Matrix.CreateTranslation(new Vector3(0, 0, 20*LevelManager.LEVEL_SIZE));
            world *= Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(world.Right, theta));
            world *= Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(world.Up, phi));

            viewport = myViewport;
            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                (float)viewport.Width /
                (float)viewport.Height,
                1, 3000);

            subWorld = world;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            Mouse.SetPosition(Game.Window.ClientBounds.Width / 2,
                Game.Window.ClientBounds.Height / 2);

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public  void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            GamePadState padState = GamePad.GetState(player);
            
            // Keyboard input
            if (keyState.IsKeyDown(Keys.Right)) { subWorld *= Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(subWorld.Up, speed / 5)); }
            if (keyState.IsKeyDown(Keys.Down)) { subWorld *= Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(subWorld.Right, speed / 5)); }
            if (keyState.IsKeyDown(Keys.Left)) { subWorld *= Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(subWorld.Down, speed / 5)); }
            if (keyState.IsKeyDown(Keys.Up)) { subWorld *= Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(subWorld.Left, speed / 5)); }

            // Controller input
            if (padState.IsButtonDown(Buttons.A))
            {
                subWorld *= Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(subWorld.Up, padState.ThumbSticks.Left.X * speed));
                subWorld *= Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(subWorld.Right, padState.ThumbSticks.Left.Y * -speed));
            }
            else
            {
                subWorld *= Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(subWorld.Up, padState.ThumbSticks.Left.X * speed / 4));
                subWorld *= Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(subWorld.Right, padState.ThumbSticks.Left.Y * -speed / 4));
            }

            // Zoom in and out with pad
            subWorld *= Matrix.CreateTranslation(subWorld.Translation / 100 * padState.Triggers.Right);
            subWorld *= Matrix.CreateTranslation(-subWorld.Translation / 100 * padState.Triggers.Left);

            // Zoom in and out with keyboard
            //if (keyState.LeftButton == ButtonState.Pressed) { subWorld *= Matrix.CreateTranslation(subWorld.Translation / 100); }
            //if (keyState.RightButton == ButtonState.Pressed) { subWorld *= Matrix.CreateTranslation(-subWorld.Translation / 100); }

            // Update focus
            focus += (targetFocus - focus) / 20.0f;
            world = subWorld * Matrix.CreateTranslation(focus);
            createLookAt();
            base.Update(gameTime);
        }

        private void createLookAt()
        {
            //view = Matrix.Invert(world);
            view = Matrix.CreateLookAt(world.Translation, focus, world.Up);
        }
    }
}
