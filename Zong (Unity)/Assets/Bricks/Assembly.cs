using UnityEngine;
using System.Collections.Generic;

public class Assembly
{
    public List<Brick> bricks;

    private enum brickLimits { MIN = 3, MAX = 8 };

    public Assembly(Color color, Int3 position, List<Int3> vacantCells, int levelSize)
    {
        bricks = new List<Brick>();
        List<Int3> occupiedList = new List<Int3>();
        int numBricks = Random.Range((int)brickLimits.MIN, (int)brickLimits.MAX);
        Int3 lastPosition;

        // The first position is always good, so let's place our first brick there
        if (Functions.cellInList(vacantCells, position) && !Functions.cellInList(occupiedList, position))
        {
            bricks.Add(NewBrick(Brick.powerUp.none, color, position));
            vacantCells.Remove(bricks[bricks.Count - 1].levelPosition);
        }
        else
        {   // Just in case =)
            throw new System.ArgumentException("Assembly start position already taken!");
        }

        // Check where to add next brick in assembly
        // Needs to check if there's a brick already there
        // Needs to check if brick is within bounds of level array
        for (int i = 0; i < numBricks; i++)
        {
            lastPosition = position;
            int nextDir = Random.Range(6, 0);
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

            if (position.X >= 0 && position.X < levelSize &&
                position.Y >= 0 && position.Y < levelSize &&
                position.Z >= 0 && position.Z < levelSize)
            {
                if (Functions.cellInList(vacantCells, position) && !Functions.cellInList(occupiedList, position))
                {
                    occupiedList.Add(position);
                    Brick.powerUp power = Brick.powerUp.none;
                    int roll = Random.Range(0,100);
                    if (roll > 90)
                    {
                        power = Brick.powerUp.multiBall;
                    }
                    bricks.Add(NewBrick(power, color, position));
                    vacantCells.Remove(bricks[bricks.Count - 1].levelPosition);
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

    private Brick NewBrick(Brick.powerUp power, Color color, Int3 position)
    {
        GameObject brickObj = (GameObject)GameObject.Instantiate(Resources.Load("Brick"));
        Brick brickScript = (Brick)brickObj.AddComponent<Brick>();
        brickScript.setParameters(power, color, position);
        return brickScript;
    }
}
