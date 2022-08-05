using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

    public Transform player;
    public Transform endPrefab;
    public Transform startPrefab;
    public Transform powerUpPrefab;
    public Transform powerUp2Prefab;
    public Transform trailMarkPrefab;

    private Transform startObj;
    private Transform endObj;

    public Level level;
    private Int3 start = new Int3(1, 1, 1);
    public Int3 end { get; private set; }
    private int deepestRecursion = 0;
    private Int3 size = new Int3(1, 1, 1) * 5;

    private HashSet<Int3> deadEnds = new HashSet<Int3>();
    private Dictionary<Int3, Transform> powerUps = new Dictionary<Int3, Transform>();
    private Dictionary<Int3, Transform> trailMarks = new Dictionary<Int3, Transform>();

    private bool markTrail = true;

    private float lengthOfSong = 3.5F;
    public Transform finalMenu;
    private bool gameEnded = false;

	// Use this for initialization
	void Start () {
        level = new Level();        
        player.GetComponent<PlayerController>().level = level; // Lazy. Should implement Singleton
        player.GetComponent<PlayerController>().levelManager = this; // Lazy. Should implement Singleton

        startObj = (Transform)Instantiate(startPrefab, Vector3.zero, Quaternion.identity);
        endObj = (Transform)Instantiate(endPrefab);

        BuildLevel(size);
        //level.destroyEdges();
    }
	
	// Update is called once per frame
	void Update () {
        /*
        if (Time.timeSinceLevelLoad > lengthOfSong) {
            // End of the game!
            if (!gameEnded) {
                gameEnded = true;
                Transform menu = (Transform)Instantiate(finalMenu);
                menu.GetComponent<GUIText>().text = "GAME OVER!\nLevel: " + level.lvl + "\nENTER to play again!";
                player.GetComponent<PlayerController>().controlEnabled = false;
            }
            if (Input.GetKeyDown(KeyCode.Return)) {
                Application.LoadLevel("game");
            }
        }
        */
	}

    public void BuildLevel(Int3 dimensions) {
        level.initLevel(dimensions);

        // Maze building algorithm: Pick a random starting point inside the maze, and dig recursively until
        //  you hit a dead end, then backtrack and choose a different route. Continue until all possibilities
        //  have been exhausted
        Dig(start, 0);
        player.transform.position = start;
        SoundManager.playEndSound(start);

        // Place some entities on the level
        startObj.position = start;
        endObj.position = end;
        bool spawnedMarkersPower = false;
        foreach (Int3 deadEnd in deadEnds) {
            if (deadEnd != end) {
                if (!spawnedMarkersPower) {
                    powerUps.Add(deadEnd, (Transform)Instantiate(powerUp2Prefab, deadEnd, Quaternion.identity));
                    spawnedMarkersPower = true;
                }
                else {
                    powerUps.Add(deadEnd, (Transform)Instantiate(powerUpPrefab, deadEnd, Quaternion.identity));
                }
                // Remove the dead ends to make this a little less linear
                foreach (Int3 dir in Utilities.dirs) {
                    if (level.canDestroyWallForContinuity(deadEnd, dir)) {
                        level.destroyWall(deadEnd + dir);
                    }
                }
            }
        }

        // Get rid of any pillars we've created
        level.destroyPillars();
        level.destroyEdges();

        Debug.Log("Finished digging. End " + deepestRecursion + " recursion levels deep and is at " + end);
    }

    // pos = starting position
    private void Dig(Int3 pos, int recursionLevel) {
        bool dug = false;
        foreach (Int3 dir in Utilities.genRandomDirs()) {
            if (level.canDestroyWallForMaze(pos, dir)) {
                if (recursionLevel == 0 && dir.y != 0) {
                    continue;
                }
                level.destroyWall(pos + dir);
                if (recursionLevel == 0) {
                    player.transform.rotation = Quaternion.LookRotation(dir);
                }
                Dig(pos + dir * 2, recursionLevel + 1);
                dug = true;
            }
        }
        if (recursionLevel > deepestRecursion) {
            end = pos;
            deepestRecursion = recursionLevel;
        }
        if (!dug) {
            deadEnds.Add(pos);
        }
    }

    public void checkPosForEvents() {
        Int3 pos = player.transform.position;
        if (pos == end) {
            Debug.Log("YOU WIN!");
            tearDownLevel();
            size += new Int3(2,2,2);
            BuildLevel(size);
        }
        if (powerUps.ContainsKey(pos) && powerUps[pos] != null) {
            addPowerUp(powerUps[pos]);
            Destroy(powerUps[pos].gameObject);
            //powerUps.Remove(pos);
            SoundManager.playPowerUpPickupSound(pos);
        }
        if (markTrail && !trailMarks.ContainsKey(pos)) {
            addTrailMark(pos);
        }
    }

    private void tearDownLevel() {
        deadEnds = new HashSet<Int3>();
        level.destroyLevel();
        foreach (Transform t in powerUps.Values) {
            if (t != null) {
                Destroy(t.gameObject);
            }
        }
        powerUps = new Dictionary<Int3, Transform>();
        foreach (Transform t in trailMarks.Values) {
            if (t != null) {
                Destroy(t.gameObject);
            }
        }
        trailMarks = new Dictionary<Int3, Transform>();
        player.GetComponent<PlayerController>().facingUp = false;
        player.GetComponent<PlayerController>().facingDown = false;
        markTrail = false;
    }

    // Roll for powerups. Markers and see-through are better, so they have a lower chance
    //  However, we should guarantee that one or the other will drop (or maybe both) per level
    private void addPowerUp(Transform powerUp) {
        if (powerUp.name.Contains("PowerUp Marker2")) {
            markTrail = true;
        }
        else {
            // add drills
            player.GetComponent<PlayerController>().numDrills += 2;
        }
    }

    private void addTrailMark(Int3 pos) {
        trailMarks.Add(pos, (Transform)Instantiate(trailMarkPrefab, pos, Quaternion.identity));
    }
}
