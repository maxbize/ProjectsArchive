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
    class Paddle2 : BasicModel
    {
        // Paddle needs the camera to get its location
        public Camera camera;

        Vector3 velocity;
        Vector3 lastPos;

        // Which player controls this pad
        PlayerIndex player;

        public Paddle2(Model m, Game game, PlayerIndex myPlayer, Viewport myViewport)
            : base(m, Functions.randomColor().ToVector3(), 0.2f, false)
        {
            camera = new Camera(game, myPlayer, myViewport);
            lastPos = camera.world.Translation;
            player = myPlayer;
        }

        public override void Update(GameTime gameTime)
        {
            camera.Update(gameTime);

            // Constrain the player to the world camera
            world = Matrix.Identity;

            world.Forward = camera.world.Forward;
            world.Right = camera.world.Right;
            world.Up = camera.world.Up;

            // Keyboard input
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.W))
            {
                world *= Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(world.Right,0.3f));
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                world *= Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(world.Right, -0.3f));
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                world *= Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(world.Up, 0.3f));
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                world *= Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(world.Up, -0.3f));
            }

            // Pad input
            GamePadState padState = GamePad.GetState(player);
            world *= Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(world.Right, padState.ThumbSticks.Right.Y / 4));
            world *= Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(world.Up, -padState.ThumbSticks.Right.X / 4));

            world *= Matrix.CreateTranslation(camera.world.Translation);

            Vector3 transVect = (world.Translation - camera.focus);
            transVect.Normalize();
            transVect *= 45;
            world *= Matrix.CreateTranslation(-transVect);

            // Set velocity
            velocity = world.Translation - lastPos;
            lastPos = world.Translation;
        }

        public Vector3 getVelocity()
        {
            return velocity;
        }
    }
}
