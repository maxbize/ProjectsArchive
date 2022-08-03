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
    public class ModelManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        List<Ball> balls = new List<Ball>();
        List<Brick2> bricks = new List<Brick2>();
        List<Paddle2> pads = new List<Paddle2>();

        public ModelManager(Game myGame)
            : base(myGame)
        {
            // Nothing to do
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {

            for (int i = 0; i < bricks.Count; i++)
            {
                bricks[i].Update(gameTime);
            }

            for (int i = 0; i < pads.Count; i++)
            {
                pads[i].Update(gameTime);
                pads[i].camera.targetFocus = getCentroid();
            }

            for (int i = balls.Count - 1; i >= 0; i--)
            {
                balls[i].Update(gameTime);
                if (balls[i].checkIfDead(pads[0].GetWorld().Translation.Length()))
                {
                    balls.RemoveAt(i);
                }
            }

            // Check for ball collisions against bricks and pads
            for (int b = 0; b < balls.Count; b++)
            {
                Ball ball = balls[b];
                BoundingSphere ballSphere = ball.model.Meshes[0].BoundingSphere.Transform(ball.GetWorld());
                int assemToDestroy = -1;
                
                // Check brick collision
                for (int i = bricks.Count - 1; i >= 0; i--)
                {
                    BoundingSphere brickSphere = bricks[i].model.Meshes[0].BoundingSphere.Transform(bricks[i].GetWorld());
                    if (ballSphere.Intersects(brickSphere))
                    {
                        if (ball.collidesWithRect(bricks[i].GetSize(), bricks[i].GetWorld()))
                        {
                            ball.brickCollision(bricks[i]);
                            assemToDestroy = bricks[i].assemblyNum;
                            //Console.WriteLine("BRICK COLLISION!");
                        }
                    }
                }
                
                // Destroy any brick assemblies that have been hit
                if (assemToDestroy != -1)
                {
                    for (int i = bricks.Count - 1; i >= 0; i--)
                    {
                        if (bricks[i].assemblyNum == assemToDestroy)
                        {
                            if (bricks[i].power == "multi-ball")
                            {
                                Vector3 vel = new Vector3((float)Functions.nextRandom(-100,101)/100,
                                    (float)Functions.nextRandom(-100,101)/100,(float)Functions.nextRandom(-100,101)/100);
                                addBall(bricks[i].GetWorld().Translation, vel);
                            }
                            bricks.RemoveAt(i);
                        }
                    }
                }

                // Generate new level if needed
                if (bricks.Count == 0)
                {
                    ((ZongGame)Game).levelManager.createLevel();
                }

                // Check collision against paddle
                for (int i = pads.Count - 1; i >= 0; i--)
                {
                    BoundingSphere padSphere = pads[i].model.Meshes[0].BoundingSphere.Transform(pads[i].GetWorld());
                    if (ballSphere.Intersects(padSphere))
                    {
                        if (ball.collidesWithRect(pads[i].GetSize(), pads[i].GetWorld()))
                        {
                            ball.padCollision(pads[i], gameTime);
                            System.Diagnostics.Debug.Write("COLLISION!\n");
                        }
                        else
                        {
                            Console.WriteLine("Sphere but not paddle collision");
                        }
                    }
                }

                // Check for collisions against other balls
                for (int j = b - 1; j >= 0; j--)
                {
                    if (ball.collidesWithBall(balls[j], gameTime))
                    {
                        ball.ballCollision(balls[j]);
                    }
                }
            }

            // Put a new ball on the paddle if necessary
            if (balls.Count == 0)
            {
                addBall(Vector3.Zero, Vector3.UnitX, 0);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Paddle2 pad in pads)
            {
                Camera cam = pad.camera;
                Game.GraphicsDevice.Viewport = cam.viewport;
                foreach (Brick2 bm in bricks)
                {
                    bm.Draw(cam);
                }

                foreach (Ball bm in balls)
                {
                    bm.Draw(cam);
                }

                foreach (BasicModel bm in pads)
                {
                    bm.Draw(cam);
                }
            }
            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public void addBall(Vector3 pos, Vector3 vel, int padToStickTo = -1)
        {
            Ball ball = new Ball(
                Game.Content.Load<Model>(@"Models\X\Ball_Dodec"),
                pos, vel);
            balls.Add(ball);
            if (padToStickTo != -1)
            {
                ball.stickToPaddle(pads[padToStickTo]);
            }
        }

        public void addPad(PlayerIndex player, Viewport viewport)
        {
            pads.Add(new Paddle2(
                Game.Content.Load<Model>(@"Models\X\Pad3"),
                Game, player, viewport));
        }

        public void addBlock(Int3 levelPos, int levelSize, int assemblyNum, Color color, String power = "none")
        {
            String model = "";
            float scale = 1f;
            if      (power == "none")       { model = @"Models\X\Cube2";}
            else if (power == "multi-ball") { model = @"Models\X\Cage"; scale = 0.1f; }
            else { throw new NotSupportedException(); }

            bricks.Add(new Brick2(
                Game.Content.Load<Model>(model),
                levelPos, levelSize, assemblyNum, color, scale, power));
        }

        private Vector3 getCentroid()
        {
            Vector3 centroid = Vector3.Zero;
            foreach (Brick2 brick in bricks)
            {
                centroid += brick.GetWorld().Translation;
            }
            if (bricks.Count > 0)
            {
                centroid /= bricks.Count;
            }
            return centroid;
        }
    }
}
