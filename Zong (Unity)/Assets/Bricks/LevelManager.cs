using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

    private static int levelSize;
    private static Vector3 centroid;

    private int maxBricksPossible;
    private int density;

    public List<Assembly> assemblies;

    // List which tells us which cells are open. Used to parse through level faster
    //  -Makes creating dense levels faster
    //  -Makes creating large, not dense levels much slower
    List<Int3> vacantCells;



	// Use this for initialization
	void Start () 
    {
        vacantCells = new List<Int3>();
        assemblies = new List<Assembly>();

        levelSize = 5;
        centroid = Vector3.zero;

        CreateLevel();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void CreateLevel()
    {
        levelSize += 2;

        Int3 start;
        ResetVacancies();
        int count = 0;

        while (GetLevelDensity() < density)
        {
            count += 1;
            start = vacantCells[Random.Range(0, vacantCells.Count - 1)];
            Assembly assem = (new Assembly(Functions.randomColor(),
                start, vacantCells, levelSize));
            assemblies.Add(assem);
            RemoveVacancies(assem);
        }
        UpdateCentroid();
    }

    private int GetLevelDensity()
    {
        int density = (int)(100 - (float)vacantCells.Count / maxBricksPossible * 100);
        return density;
    }

    private void RemoveVacancies(Assembly assem)
    {
        foreach (Brick brick in assem.bricks)
        {
            vacantCells.Remove(brick.levelPosition);
        }
    }

    private void ResetVacancies()
    {
        vacantCells = new List<Int3>();
        int type = Random.Range(0, 3);
        density = 35;

        // Creates Cube-like level
        if (type == 0)
        {
            for (int i = 0; i < levelSize; i++)
            {
                for (int j = 0; j < levelSize; j++)
                {
                    for (int k = 0; k < levelSize; k++)
                    {
                        vacantCells.Add(new Int3(i, j, k));
                    }
                }
            }
        }

        // Spherical level
        else if (type == 1)
        {
            Int3 center = new Int3(levelSize / 2, levelSize / 2, levelSize / 2);
            int radius = levelSize / 2;
            for (int i = 0; i < levelSize; i++)
            {
                for (int j = 0; j < levelSize; j++)
                {
                    for (int k = 0; k < levelSize; k++)
                    {
                        Int3 cell = new Int3(i, j, k);
                        if ((cell - center).Length() <= radius)
                        {
                            vacantCells.Add(new Int3(i, j, k));
                        }
                    }
                }
            }
        }

        // Scripted level
        else if (type == 2)
        {
            type = Random.Range(0, 0);
            if (type == 0) // Hollow cube
            {
                density = 80;
                int edge = levelSize - 1;
                for (int i = 0; i < levelSize; i++)
                {
                    for (int j = 0; j < levelSize; j++)
                    {
                        for (int k = 0; k < levelSize; k++)
                        {
                            if (i == 0 || j == 0 || k == 0 ||
                                i == edge || j == edge || k == edge)
                            {
                                vacantCells.Add(new Int3(i, j, k));
                            }
                        }
                    }
                }
            }
        }
        maxBricksPossible = vacantCells.Count;
    }

    private void UpdateCentroid()
    {
        int numBricks = 0;
        centroid = Vector3.zero;
        foreach (Assembly assem in assemblies)
        {
            numBricks += assem.bricks.Count;
            foreach (Brick brick in assem.bricks) 
            {
                centroid += brick.transform.position;
            }
        }
        if (numBricks > 0)
        {
            centroid /= numBricks;
        }
        print(centroid);
        print(assemblies.Count + " " + numBricks);
    }

    // Call this function whenever an assembly has been hit and needs to be removed
    // TODO: Find a better way to get the assembly
    public void RemoveAssembly(Int3 brickPos)
    {
        bool found = false;
        for (int i = assemblies.Count - 1; i >= 0; i--)
        {
            foreach (Brick brick in assemblies[i].bricks)
            {
                if (brick.levelPosition == brickPos)
                {
                    found = true;
                    break;
                }
            }
            if (found)
            {
                foreach (Brick brick in assemblies[i].bricks)
                {
                    Destroy(brick.gameObject);
                }
                assemblies.RemoveAt(i);
                UpdateCentroid();
                break;
            }
        }
        if (!found)
        {
            //print("Brick not found at levelPos: " + brickPos + "!");
            // We can't throw an exception here because this situation can happen easily.
            //  If the ball hits two bricks from the same assembly in the same frame, then 
            //  the assembly will no longer exist for the second find.
        }
        if (assemblies.Count == 0)
        {
            CreateLevel();
        }
       
    }

    public static Vector3 GetCentroid()
    {
        return centroid;
    }

    public static float GetLevelSpeed()
    {
        return levelSize * 2f;
    }
}
