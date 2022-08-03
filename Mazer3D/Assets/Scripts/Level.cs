using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level
{

    private Grid grid;
    public Int3 size { get; private set; }

    public UnityEngine.Object innerWallPrefab;
    public UnityEngine.Object outerWallPrefab;

    private GameObject boundingBox;

    public int lvl = 0;

    public Level() {
        innerWallPrefab = Resources.Load("InnerWall");
        outerWallPrefab = Resources.Load("OuterWall");

        size = new Int3(0, 0, 0);
    }

    // Works best with odd numbered sizes!
    public void initLevel(Int3 size) {
        this.size = size;
        grid = new Grid(size);

        for (int i = 0; i < size.x; i++) {
            for (int j = 0; j < size.y; j++) {
                for (int k = 0; k < size.z; k++) {
                    SpawnWall(new Int3(i, j, k));
                }
            }
        }

        lvl++;
    }


    public void destroyLevel() {
        for (int i = 0; i < size.x; i++) {
            for (int j = 0; j < size.y; j++) {
                for (int k = 0; k < size.z; k++) {
                    grid.destroyAt(new Int3(i, j, k));
                }
            }
        }
        GameObject.Destroy(boundingBox);
    }


    // Inner Grid class for more easily accessing a 3-D array
    class Grid
    {
        private GameObject[, ,] innerGrid;

        public Grid(Int3 size) {
            innerGrid = new GameObject[size.x, size.y, size.z];
        }

        public GameObject this[Int3 i] {
            get { return innerGrid[i.x, i.y, i.z]; }
            set { innerGrid[i.x, i.y, i.z] = value; }
        }

        public void destroyAt(Int3 i) {
            if (this[i] != null) {
                GameObject.Destroy(this[i]);
                this[i] = null;
            }
        }
    }

    private void SpawnWall(Int3 pos) {
        if (pos.x % 2 == 0 || pos.y % 2 == 0 || pos.z % 2 == 0) {
            GameObject wallObj;
            if (wallOnEdge(pos)) {
                wallObj = (GameObject)GameObject.Instantiate(outerWallPrefab, pos, Quaternion.identity);

            }
            else {
                wallObj = (GameObject)GameObject.Instantiate(innerWallPrefab, pos, Quaternion.identity);
            }
            grid[pos] = wallObj;
        }
    }

    // Can we destroy the wall for mazing purposes. I.e. is the cell beyond the wall we're destroying unvisited?
    public bool canDestroyWallForMaze(Int3 cellPos, Int3 dirToWall) {
        if (!canDestroyWall(cellPos + dirToWall)) {
            return false;
        }
        Int3 connectedCell = cellPos + (dirToWall * 2);
        return cellIsIsolated(connectedCell);
    }

    // Can we destroy the wall and connect to another part of the maze?
    public bool canDestroyWallForContinuity(Int3 cellPos, Int3 dirToWall) {
        if (!canDestroyWall(cellPos + dirToWall)) {
            return false;
        }
        Int3 connectedCell = cellPos + (dirToWall * 2);
        if (grid[connectedCell] != null) {
            return false;
        }
        return true;
    }

    // Can we destroy the wall, period!
    public bool canDestroyWall(Int3 pos) {
        if (wallOnEdge(pos) ||
            grid[pos] == null) {
            return false;
        }
        return true;
    }

    // A cell is isolated if it has walls on all sides
    public bool cellIsIsolated(Int3 pos) {
        foreach (Int3 dir in Utilities.dirs) {
            if (grid[pos + dir] == null) {
                return false;
            }
        }
        return true;
    }

    public void destroyWall(Int3 pos) {
        grid.destroyAt(pos);
    }

    // This just looks cooler =) Temporary for now
    public void destroyEdges() {
        for (int i = 0; i < size.x; i++) {
            for (int j = 0; j < size.y; j++) {
                for (int k = 0; k < size.z; k++) {
                    if (wallOnEdge(new Int3(i, j, k))) {
                        destroyWall(new Int3(i, j, k));
                    }
                }
            }
        }
        // Replace with a bounding box
        boundingBox = (GameObject)GameObject.Instantiate(Resources.Load<GameObject>("Bounding Box"));
        boundingBox.transform.position = (Vector3)(size - new Int3(1, 1, 1)) / 2;
        boundingBox.transform.localScale = (Vector3)(size - new Int3(2, 2, 2));
    }

    public bool canMoveHere(Int3 pos) {
        try {
            if (grid[pos] == null && !wallOnEdge(pos)) {
                return true;
            }
            return false;
        }
        catch (System.IndexOutOfRangeException) {
            return false;
        }
    }

    private bool wallOnEdge(Int3 pos) {
        if (
                pos.x == 0 ||
                pos.y == 0 ||
                pos.z == 0 ||
                pos.x == size.x - 1 ||
                pos.y == size.y - 1 ||
                pos.z == size.z - 1
                ) {
            return true;
        }
        return false;
    }

    // "pillars" are walls that have openings on 4 co-planar sides
    //  Don't bother iterating through the edges, we won't find any there.
    public void destroyPillars() {
        for (int i = 1; i < size.x - 1; i++) {
            for (int j = 1; j < size.y - 1; j++) {
                for (int k = 1; k < size.z - 1; k++) {
                    Int3 pos = new Int3(i, j, k);
                    if (wallOnEdge(pos)) {
                        continue;
                    }
                    int numAdjacentWalls = 0;
                    // With the current algo this is only possible on the x-y plane, so just do one pass!
                    foreach (Int3 dir in Utilities.dirs) {
                        if (grid[pos + dir] != null) {
                            numAdjacentWalls++;
                        }
                    }
                    if (numAdjacentWalls <= 2) {
                        destroyWall(pos);
                    }
                    //  We're going to be a little more liberal and do 3 openings to open up the maps a little
                    if (numAdjacentWalls == 3 && Random.Range(0,2) > 1) {
                        destroyWall(pos);
                    }
                }
            }
        }
    }
}
