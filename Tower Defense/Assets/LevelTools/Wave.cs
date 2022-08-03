using UnityEngine;
using System.Collections;

public struct Wave {

    // enemies
    public float speed;
    public float health;
    public int gold;
    public bool flying;

    // wave
    public int numEnemies;
    public float frequency;

    public Wave(float speed, float health, int gold, bool flying,
        int numEnemies, float frequency)
    {
        this.speed = speed;
        this.health = health;
        this.gold = gold;
        this.flying = flying;
        this.numEnemies = numEnemies;
        this.frequency = frequency;
    }
}

