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
    public class Assembly
    {
        // A list containing the brick locations of this assembly
        List<Int3> occupiedList;
        int assemblyNum;

        public Assembly(Color color, Int3 position, List<Int3> vacantCells, ModelManager modelMnger, int asmblyNum)
        {
            assemblyNum = asmblyNum;
            occupiedList = new List<Int3>();
            int numBricks = LevelManager.rnd.Next(5) + 3;
            Int3 lastPosition;
            int hue = LevelManager.rnd.Next(-4,4);
            if (hue == 0)
            {
                hue += 1;
            }

            // Check where to add next brick in assembly
            // Needs to check if there's a brick already there
            // Needs to check if brick is within bounds of level array
            for (int i = 0; i < numBricks; i++)
            {
                lastPosition = position;
                int nextDir = LevelManager.rnd.Next(6);
                switch (nextDir)
                {
                case 0:
                    position.X += 1;
                    break;
                case 1:
                    position.X -= 1;
                    break;
                case 2:
                    position.Y += 1;
                    break;
                case 3:
                    position.Y -= 1;
                    break;
                case 4:
                    position.Z += 1;
                    break;
                case 5:
                    position.Z -= 1;
                    break;
                }

                if (position.X >= 0 && position.X < LevelManager.LEVEL_SIZE &&
                    position.Y >= 0 && position.Y < LevelManager.LEVEL_SIZE &&
                    position.Z >= 0 && position.Z < LevelManager.LEVEL_SIZE)
                {
                    if (Functions.cellInList(vacantCells, position) && !Functions.cellInList(occupiedList,position))
                    {
                        occupiedList.Add(position);
                        String power = "none";
                        int roll = Functions.nextRandom();
                        if (roll > 90)
                        {
                            power = "multi-ball";
                        }
                        modelMnger.addBlock(position, LevelManager.LEVEL_SIZE, assemblyNum, color, power);
                    }
                    else
                    {
                        position = lastPosition;
                    }
                }
                else
                {
                    position = lastPosition;
                }
            }
        }


        public List<Int3> getOccupied()
        {
            return occupiedList;
        }
    }
}
