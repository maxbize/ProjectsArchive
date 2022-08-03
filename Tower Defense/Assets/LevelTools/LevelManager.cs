using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public List<Enemy> enemies;
    public Level currentLevel;
    Wave currentWave;
    int levelIndex = -1; // Start at -1 so that we can call nextLevel right away
    public int waveIndex = 0;
    int numSpawnedEnemies = 0;
    float waitTime = 0;
    BuildManager buildManagerRef;
    Player player;

    // Use this for initialization
    void Start()
    {
        buildManagerRef = GameObject.Find("Build Manager").GetComponent<BuildManager>();
        player = GameObject.Find("Player Manager").GetComponent<Player>();        
        StartNextLevel();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (numSpawnedEnemies < currentWave.numEnemies
            && waitTime > currentWave.frequency)
        {
            SpawnEnemy();
        }
        else
        {
            waitTime += Time.deltaTime;
        }
    }

    void SpawnEnemy()
    {
        GameObject newEnemy = (GameObject)GameObject.Instantiate(GameObject.Find("Cloneable_EnemyOne"));
        Enemy enemyScriptObj = newEnemy.AddComponent<Enemy>();
        enemies.Add(enemyScriptObj);
        numSpawnedEnemies += 1;
        waitTime = 0;
    }

    public void StartNextWave()
    {
        waveIndex += 1;
        numSpawnedEnemies = 0;
        currentWave = currentLevel.waves[waveIndex];
    }

    void StartNextLevel()
    {
        levelIndex += 1;
        currentLevel = LevelData.GetLevel(levelIndex);
        buildManagerRef.ProcessNewLevel(currentLevel);
    }

    // Call this whenever an enemy has reached the exit
    public void EnemyExit(Enemy enemy)
    {
        player.lives -= 1;
        enemies.Remove(enemy);
    }

    // Call this whenever an enemy gets killed
    public void enemyDeath(Enemy enemy)
    {
        player.gold += currentWave.gold;
        enemies.Remove(enemy);
    }
}
