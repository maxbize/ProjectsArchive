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
    class Brick2 : BasicModel
    {
        float brickSize;
        public Int3 levelPosition;
        public int assemblyNum;
        public string power;

        public Brick2(Model m, Int3 levelPos, int levelSize, int asmblyNum, Color color, float scale, string pow)
            : base(m, color.ToVector3())
        {
            power = pow;
            assemblyNum = asmblyNum;
            levelPosition = levelPos;
            brickSize = GetSize().X * 2;
            
            float xPos = (levelPosition.X - levelSize / 2.0f + 0.5f) * brickSize;
            float yPos = (levelPosition.Y - levelSize / 2.0f + 0.5f) * brickSize;
            float zPos = (levelPosition.Z - levelSize / 2.0f + 0.5f) * brickSize;

            world *= Matrix.CreateTranslation(new Vector3(xPos, yPos, zPos));
            world *= Matrix.CreateScale(scale);
        }
    }
}

