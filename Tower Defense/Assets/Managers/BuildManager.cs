using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    Player player;
    GameObject buildMarker;
    List<Vector3> invalidPositions = new List<Vector3>();
    GameObject towerBeingBuilt = null;
    Vector3 towerPoolPosition;
    Vector3 objectPoolPosition = new Vector3(0, 600, 0);

    GameObject greenRangeMarker;
    GameObject redRangeMarker;
    GameObject upgradeArrow;

    public Dictionary<Tower.type_t, GameObject> towerInstances = new Dictionary<Tower.type_t, GameObject>();

    GUIManager guiManager;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player Manager").GetComponent<Player>();
        guiManager = GameObject.Find("GUI Manager").GetComponent<GUIManager>();
        buildMarker = GameObject.Find("Build Marker");
        buildMarker.transform.position = objectPoolPosition;
        greenRangeMarker = GameObject.Find("Tower Range Green");
        redRangeMarker = GameObject.Find("Tower Range Red");
        upgradeArrow = GameObject.Find("Upgrade Arrow");

        towerInstances.Add(Tower.type_t.iceTower, GameObject.Find("Cloneable_IceTower"));
        towerInstances.Add(Tower.type_t.fireTower, GameObject.Find("Cloneable_FireTower"));
        towerInstances.Add(Tower.type_t.laserTower, GameObject.Find("Cloneable_LaserTower"));
        towerInstances.Add(Tower.type_t.bombTower, GameObject.Find("Cloneable_BombTower"));
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            switchTowerToBuild(Tower.type_t.iceTower);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            switchTowerToBuild(Tower.type_t.laserTower);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            switchTowerToBuild(Tower.type_t.bombTower);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            switchTowerToBuild(Tower.type_t.fireTower);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            switchTowerToBuild(Tower.type_t.none);
        } 
        

        if (towerBeingBuilt)
        {
            Vector3 mouseHit = GetMouseHitPos();
            if (mouseHit != Vector3.zero && (PlaceBuildMarker(mouseHit)
                && Input.GetKeyDown(KeyCode.Mouse0)))
            { 
                BuildTower(mouseHit);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            GameObject mouseHit = GetMouseObjectHit();
            if (mouseHit)
            {
                Tower towerHit = mouseHit.GetComponent<Tower>();
                if (towerHit)
                {
                    guiManager.ActivateTowerMenu(towerHit);
                    previewRange(towerHit);
                }
                else if (mouseHit == upgradeArrow)
                {
                    print("Upgrading!");

                    guiManager.UpgradeActiveTower();
                }

                else
                {
                    greenRangeMarker.transform.position = objectPoolPosition;
                    redRangeMarker.transform.position = objectPoolPosition;
                    upgradeArrow.transform.position = objectPoolPosition;
                }
            }
        }
    }

    bool PlaceBuildMarker(Vector3 pos)
    {
        pos = GetGridPos(pos);
        buildMarker.transform.position = pos + Vector3.up;
        if (invalidPositions.Contains(pos))
        {
            towerBeingBuilt.transform.position = towerPoolPosition;
            buildMarker.GetComponentInChildren<Renderer>().material.color = Color.red;
            greenRangeMarker.transform.position = towerPoolPosition;
            redRangeMarker.transform.position = towerPoolPosition;
            return false;
        }
        else
        {
            towerBeingBuilt.transform.position = pos;
            buildMarker.GetComponentInChildren<Renderer>().material.color = Color.green;
            greenRangeMarker.transform.position = pos;
            redRangeMarker.transform.position = pos + Vector3.up;
            return true;
        }
    }

    Vector3 GetGridPos(Vector3 pos)
    {
        float gridX = (int)(pos.x / 100) * 100 + (pos.x > 0 ? 50 : -50);
        float gridZ = (int)(pos.z / 100) * 100 + (pos.z > 0 ? 50 : -50);
        return new Vector3(gridX, 35, gridZ); // Need to get above the terrain
    }

    public void ProcessNewLevel(Level level)
    {
        // Populate invalid positions from the game grid
        for (int i = 0; i < level.markers.Count - 1; i++)
        {
            Vector3 markToMark = level.markers[i + 1] - level.markers[i];
            int distance = 10;
            while (distance < markToMark.magnitude)
            {
                invalidPositions.Add(GetGridPos(level.markers[i] + markToMark.normalized * distance));
                distance += 100;
            }
        }
        // Need to add the last levelMarker manually
        invalidPositions.Add(GetGridPos(level.markers[level.markers.Count - 1]));
    }

    void BuildTower(Vector3 pos)
    {
        GameObject tower = (GameObject)GameObject.Instantiate(towerBeingBuilt); // Clone the tower
        Tower towerScript = tower.GetComponent<Tower>();
        player.gold -= towerScript.baseCost;
        towerScript.Activate();
        invalidPositions.Add(GetGridPos(pos));
        towerBeingBuilt.transform.position = towerPoolPosition;
        towerBeingBuilt = null;
        buildMarker.transform.position = objectPoolPosition;
        greenRangeMarker.transform.position = objectPoolPosition;
        redRangeMarker.transform.position = objectPoolPosition;
    }

    public void switchTowerToBuild(Tower.type_t towerType)
    {
        if (towerBeingBuilt)
        {
            towerBeingBuilt.transform.position = towerPoolPosition;
        }
        if (towerType == Tower.type_t.none) // Cancel current build
        {
            towerBeingBuilt = null;
            buildMarker.transform.position = objectPoolPosition;
            greenRangeMarker.transform.position = objectPoolPosition;
            redRangeMarker.transform.position = objectPoolPosition;
        }
        else
        {
            towerBeingBuilt = towerInstances[towerType];
            towerPoolPosition = towerBeingBuilt.transform.position;
            float maxRange = towerBeingBuilt.GetComponent<Tower>().maxRange * 2; // Range is radial, scale is diametrical
            float minRange = towerBeingBuilt.GetComponent<Tower>().minRange * 2; // Range is radial, scale is diametrical
            greenRangeMarker.transform.localScale = new Vector3(maxRange, 0.2f, maxRange);
            redRangeMarker.transform.localScale = new Vector3(minRange, 0.2f, minRange);
        }
    }

    public Vector3 GetMouseHitPos()
    {
        Ray camToMouse;
        RaycastHit rayCastHit;

        camToMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(camToMouse, out rayCastHit, Mathf.Infinity))
        {
            return rayCastHit.point;
        }
        return Vector3.zero;
    }

    GameObject GetMouseObjectHit()
    {
        Ray camToMouse;
        RaycastHit rayCastHit;

        camToMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(camToMouse, out rayCastHit, Mathf.Infinity))
        {
            return rayCastHit.transform.gameObject;
        }
        return null;
    }

    public Tower getBaseTowerScript(Tower.type_t type)
    {
        return towerInstances[type].GetComponent<Tower>();
    }

    public void previewRange(Tower tower)
    {
        greenRangeMarker.transform.localScale = new Vector3(tower.maxRange, 0.1f, tower.maxRange) * 2;
        greenRangeMarker.transform.position = GetGridPos(tower.transform.position);
        redRangeMarker.transform.localScale = new Vector3(tower.minRange, 0.1f, tower.minRange) * 2;
        redRangeMarker.transform.position = GetGridPos(tower.transform.position) + new Vector3(0,0.1f,0); // Need the red to be a little higher
    }
}
