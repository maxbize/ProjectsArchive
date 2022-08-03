using UnityEngine;
using System.Collections.Generic;
using System;


public class GUIManager : MonoBehaviour
{
    Player player; // Needed to get gold / lives
    LevelManager levelManager; // Needed to get level info (wave #, enemies remaining, etc.)
    BuildManager buildManager; // Needed to trigger building of towers
    Tower currentTower; // The tower we are building or upgrading, as per activeMenu
    GameObject upgradeArrow; // Arrow user clicks to upgrade tower

    Vector3 arrowOffset = new Vector3(45, 40, 40);

    enum menu_t { none, build, upgrade }
    menu_t activeMenu = menu_t.none;

    // For hovering / build preview
    Dictionary<string, Tower.type_t> towerDict = new Dictionary<string, Tower.type_t>();
    string iceTowStr = "Ice Tower";
    string fireTowStr = "Fire Tower";
    string laserTowStr = "Laser Tower";
    string bombTowStr = "Bomb Tower";


    void Start()
    {
        player = GameObject.Find("Player Manager").GetComponent<Player>();
        levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        buildManager = GameObject.Find("Build Manager").GetComponent<BuildManager>();
        upgradeArrow = GameObject.Find("Upgrade Arrow");

        towerDict.Add(iceTowStr, Tower.type_t.iceTower);
        towerDict.Add(fireTowStr, Tower.type_t.fireTower);
        towerDict.Add(laserTowStr, Tower.type_t.laserTower);
        towerDict.Add(bombTowStr, Tower.type_t.bombTower);
    }

    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width - 200, 0, 200, 450), ""); // Main box for GUI
        GUILayout.BeginArea(new Rect(Screen.width - 200, 0, 200, 450));
        GUILayout.BeginVertical();

        GUILayout.Space(20);

        // All of the stuff that's in all menus 
        GUILayout.Box("Lives: " + player.lives);
        GUILayout.Box("Gold: " + player.gold);
        GUILayout.Box("Wave: " + (levelManager.waveIndex));
        if (GUILayout.Button("Next Wave"))
        {
            levelManager.StartNextWave();
        }

        GUILayout.Space(20);

        // All of the build stuff
        foreach (string towerStr in towerDict.Keys)
        {
            if (GUILayout.Button(new GUIContent(towerStr, towerStr)))
            {
                buildManager.switchTowerToBuild(towerDict[towerStr]);
            }
        }

        GUILayout.Space(20);
        if (activeMenu == menu_t.build)
        {
            GUILayout.Box(currentTower.generalStr);
            GUILayout.Box("Cost: " + currentTower.baseCost);
            GUILayout.Box("Damage: " + currentTower.damageStr);
            GUILayout.Box("Speed: " + currentTower.speedStr);
            GUILayout.Box("Range: " + currentTower.rangeStr);
            GUILayout.Box("Effect: " + currentTower.effectStr);
        }

            /*
        else if (activeMenu == menu_t.upgrade)
        {
            switch (currentTower.type)
            {
                case Tower.type_t.iceTower:
                    ((IceTower)currentTower).PopulateUpgradeMenu(this);
                    break;
                case Tower.type_t.fireTower:
                    ((FireTower)currentTower).PopulateUpgradeMenu(this);
                    break;
                case Tower.type_t.laserTower:
                    ((LaserTower)currentTower).PopulateUpgradeMenu(this);
                    break;
                case Tower.type_t.bombTower:
                    ((BombTower)currentTower).PopulateUpgradeMenu(this);
                    break;
            }
        }
        */

        GUILayout.EndVertical();
        GUILayout.EndArea();

        // This must be done last or onGUI will crash
        if (GUI.tooltip != "")
        {
            ActivateBuildMenu(towerDict[GUI.tooltip]); // mouse over
        }
    }

    public void ActivateBuildMenu(Tower.type_t towerType)
    {
        currentTower = buildManager.getBaseTowerScript(towerType);
        activeMenu = menu_t.build;
    }

    public void ActivateTowerMenu(Tower tower)
    {
        currentTower = tower;
        upgradeArrow.transform.position = currentTower.transform.position + arrowOffset;
        activeMenu = menu_t.upgrade;
    }

    public void ClearActiveMenu()
    {
        activeMenu = menu_t.none;
    }

    public void AddUpgrade(string upgradeText, int upgradeLevel, int cost, Action upgradeFunction)
    {
        upgradeText = upgradeText + (upgradeLevel > 5 ? "MAXED" : upgradeLevel.ToString());
        GUILayout.Box(upgradeText);
        if (upgradeLevel <= 5)
        {
            if (GUILayout.Button("Upgrade: " + cost))
            {
                HandleUpgrade(cost, upgradeFunction);
                buildManager.previewRange(currentTower);
            }
        }
        else
        {
            GUILayout.Box("Can't upgrade");
        }
        GUILayout.Space(30);
    }

    void HandleUpgrade(int cost, Action upgradeFunction)
    {
        //if (player.gold >= cost) // Don't need this while testing
        {
            player.gold -= cost;
            upgradeFunction();
        }
    }

    public void UpgradeActiveTower()
    {
        print(currentTower.name);
        HandleUpgrade(currentTower.upgradeCost, currentTower.Upgrade);
    }
}
