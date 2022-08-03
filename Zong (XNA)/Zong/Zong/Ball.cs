using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// Keyboard input for debugging
using Microsoft.Xna.Framework.Input;

/* Notes:
 *  Ball radius in 3DS MAX is 2.5
 */

namespace Zong
{
    class Ball : BasicModel
    {
        Vector3 velocity;
        float radius;
        TimeSpan lastPadBounce = new TimeSpan();
        TimeSpan lastBallBounce = new TimeSpan();
        Paddle2 stickyPaddle;
        bool isStuck;

        Vector3 rotationVect = Vector3.Zero;
        float angularVel = 0;

        // Keep track of the last collided ball to avoid balls sticking to each other
        Ball lastCollided;

        public Ball(Model m, Vector3 pos, Vector3 vel)
            : base(m, Functions.randomColor().ToVector3())
        {
            world *= Matrix.CreateTranslation(pos);
            velocity = vel;
            radius = this.model.Meshes[0].BoundingSphere.Radius;
        }

        public override void Update(GameTime gameTime)
        {
            // Rotate ball
            if (rotationVect.X != float.NaN)
            {
                Vector3 position = world.Translation;
                world.Translation = Vector3.Zero;
                world *= Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(rotationVect, angularVel));
                world *= Matrix.CreateTranslation(position);
            }

            // Translate ball (velocity + spin)
            world *= Matrix.CreateTranslation(velocity);
            world *= Matrix.CreateTranslation(Vector3.Cross(rotationVect,velocity) * angularVel * 5);
            angularVel *= 0.99f;

            // Bound ball to paddle
            if (isStuck)
            {
                Vector3 offset = stickyPaddle.GetWorld().Translation;
                offset.Normalize();
                world.Translation = stickyPaddle.GetWorld().Translation - offset * radius * 1.1f;

                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                KeyboardState keyState = Keyboard.GetState(PlayerIndex.One);
                if (gamePadState.IsButtonDown(Buttons.B) || keyState.IsKeyDown(Keys.Space))
                {
                    isStuck = false;
                    alpha = 1;
                }
            }

            base.Update(gameTime);
        }

        public bool checkIfDead(float padDistance)
        {
            if (world.Translation.Length() > 1.1f * padDistance)
            {
                return true;
            }
            return false;
        }

        // Add a sticky relation to a paddle. Maintaining the relation is handled in Ball.Update()
        public void stickToPaddle(Paddle2 paddle)
        {
            isStuck = true;
            stickyPaddle = paddle;
            alpha = 0.8f;
        }

        public bool collidesWithRect(Vector3 rectSizes, Matrix rectWorld)
        {
            Vector3 distance = world.Translation - rectWorld.Translation;
            Vector3 direction = distance;
            direction.Normalize();

            float dxp = Vector3.Dot((rectSizes.X * rectWorld.Right), direction);
            float dyp = Vector3.Dot((rectSizes.Y * rectWorld.Forward), direction);
            float dzp = Vector3.Dot((rectSizes.Z * rectWorld.Up), direction);
            double dRect = Math.Sqrt(dxp * dxp + dyp * dyp + dzp * dzp);

            if (radius + dRect >= distance.Length())
            {
                return true;
            }
            return false;
        }

        // Calculate new velocity
        public void brickCollision(Brick2 brick)
        {
            Vector3 oldVel = velocity;
            Vector3 brickTranslation = brick.GetWorld().Translation;
            float deltaX = Math.Abs(brickTranslation.X - world.Translation.X);
            float deltaY = Math.Abs(brickTranslation.Y - world.Translation.Y);
            float deltaZ = Math.Abs(brickTranslation.Z - world.Translation.Z);
            float biggest = Math.Max(Math.Max(deltaX, deltaY),deltaZ);
            if (biggest == deltaX)
            {
                velocity.X *= -1;
            }
            else if (biggest == deltaY)
            {
                velocity.Y *= -1;
            }
            else if (biggest == deltaZ)
            {
                velocity.Z *= -1;
            }
            else
            {
                // This should never get called
                velocity = Vector3.Zero;
            }

            // Set rotation vector, speed
            rotationVect = Vector3.Cross(velocity, oldVel);
            rotationVect.Normalize();
            double theta = Math.Acos((Vector3.Dot(oldVel, velocity) / ((oldVel.Length() * velocity.Length()) * 1.0001f))); // Need a slight bump to avoid domain error
            angularVel = (velocity.Length() / radius * (float)Math.Cos(theta/2)) / 2;
        }

        // Calculate new velocity / spin
        public void padCollision(Paddle2 pad, GameTime gameTime)
        {
            TimeSpan time = gameTime.TotalGameTime.Duration();
            TimeSpan minTimeBtwnCollisions = new TimeSpan(0, 0, 0, 0, 200);
            if ((time - lastPadBounce) > minTimeBtwnCollisions)
            {
                Console.WriteLine("BOUNCED!");
                lastPadBounce = time;

                // Testing alternate method: Just make the ball bounce to where the paddle is facing
                Matrix padWorld = pad.GetWorld();
                float xNew = Vector3.Dot(padWorld.Forward, Vector3.Right);
                float yNew = Vector3.Dot(padWorld.Forward, Vector3.Up);
                float zNew = Vector3.Dot(padWorld.Forward, Vector3.Backward);
                float oldSpeed = velocity.Length();
                velocity = new Vector3(xNew, yNew, zNew);
                velocity *= oldSpeed;

                // Set rotation vector, speed
                rotationVect = pad.getVelocity();
                if (rotationVect.Length() == 0)
                {
                    rotationVect = Vector3.Right;
                    angularVel = 0;
                }
                else
                {
                    rotationVect = Vector3.Cross(rotationVect, padWorld.Forward);
                    rotationVect.Normalize();
                    Console.WriteLine("Pad speed: " + pad.getVelocity().Length());
                    //angularVel = (pad.getVelocity().Length()) *0.05f;
                    float speed = pad.getVelocity().Length();
                    angularVel = -0.0038f * speed * speed + 0.0561f * speed;
                    Console.WriteLine("Angular: " + angularVel);
                }
            }
        }

        // Checks if two balls are colliding
        public bool collidesWithBall(Ball b, GameTime gameTime)
        {
            TimeSpan time = gameTime.TotalGameTime.Duration();
            TimeSpan minTimeBtwnCollisions = new TimeSpan(0, 0, 0, 0, 200);
            float dist = (b.GetWorld().Translation - GetWorld().Translation).Length();
            // if colliding && ((colliding with last && enough time has passed) || not colliding with last)
            if (dist < 2 * radius && ((lastCollided != b) || (lastCollided == b && (time - lastBallBounce > minTimeBtwnCollisions))))
            {
                lastBallBounce = time;
                lastCollided = b;
                return true;
            }
            return false;
        }

        // Alters velocities of both balls for collision
        public void ballCollision(Ball b)
        {
            /* Only the normal component of the velocity will change during collision. Assume
             *  that both masses are equal, which means that the normal component of one ball after
             *  the collision is equal to the normal component of velocity of the other ball before collision.
             */
            Vector3 unitNorm = b.GetWorld().Translation - GetWorld().Translation;
            unitNorm.Normalize();
            Vector3 u1Norm = Vector3.Dot(velocity,   unitNorm) * unitNorm;
            Vector3 u2Norm = Vector3.Dot(b.velocity, unitNorm) * unitNorm;
            Vector3 u1Tan =   velocity - unitNorm * (Vector3.Dot(  velocity, unitNorm));
            Vector3 u2Tan = b.velocity - unitNorm * (Vector3.Dot(b.velocity, unitNorm));
            velocity   = u1Tan + u2Norm;
            b.velocity = u2Tan + u1Norm;
        }
    }
}
