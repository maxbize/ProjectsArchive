using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    LevelManager levelManagerRef; // Need this to chain off any level events, such as reaching the end
    List<Vector3> levelMarkers;
    int nextMarker = 0; // Points to the next marker that the enemy will travel over
    public float markerDistanceLastFrame; // Distance to next marker from last frame
    float speed = 100.0f;
    const float MAX_HEALTH = 100f;
    float health = MAX_HEALTH;

    // health bar
    GameObject healthBar;
    Vector3 healthBarPosOffset = Vector3.up * 30;


    public FireDOT fireDOT; // Non-null when taking fire damage over time

    // Use this for initialization
    void Start()
    {
        // Logic initialisation
        levelManagerRef = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        levelMarkers = levelManagerRef.currentLevel.markers;
        gameObject.GetComponent<Rigidbody>().transform.position = levelMarkers[nextMarker] + 20 * Vector3.up;
        markerDistanceLastFrame = 1E20f; // Just initialize this to a very high value so that we don't register a turn in frame 1
        Turn();

        // Initialize health bar
        healthBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
        healthBar.transform.localScale *= 10;
        healthBar.transform.position = transform.position;
        Destroy(healthBar.GetComponent<BoxCollider>());
        UpdateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.transform.position = transform.position + healthBarPosOffset;
        UpdateHealthBar();
        float currentMarkerDistance = (levelMarkers[nextMarker] - gameObject.transform.position).magnitude;
        if (currentMarkerDistance > markerDistanceLastFrame)
        {
            Turn();
            markerDistanceLastFrame = 1E20f; // Don't trigger another turn
        }
        else
        {
            markerDistanceLastFrame = currentMarkerDistance;
        }
    }

    // Process a turn signal: turn the model and augment marker index
    void Turn()
    {
        int lastMarker = nextMarker;
        nextMarker++;

        Vector3 lastMarker_xz = new Vector3(levelMarkers[lastMarker].x, transform.position.y, levelMarkers[lastMarker].z);
        transform.position = lastMarker_xz;

        if (levelMarkers.Count == nextMarker)
        {
            // Enemy has reached the exit
            levelManagerRef.EnemyExit(this);
            Destroy(healthBar);
            Destroy(gameObject);
        }
        else
        {
            Quaternion lookRotation = new Quaternion();
            Vector3 direction = levelMarkers[nextMarker] - levelMarkers[lastMarker];
            lookRotation.SetLookRotation(direction);
            gameObject.transform.rotation = lookRotation;
            gameObject.GetComponent<Rigidbody>().velocity = direction.normalized * speed;
        }
    }

    public void registerHit(float damage)
    {
        health -= damage;
        UpdateHealthBar();
        if (health < 0)
        {
            levelManagerRef.enemyDeath(this);
            Destroy(healthBar);
            Destroy(gameObject);
        }
    }

    // Current flaw: If the first next marker is at a different altitude the change in altitude is ignored
    // Use max turns if you want to limit the accuracy of a tower (e.g. bomb tower)
    public Vector3 forecastPosition(float t, int maxTurns = 10000)
    {

        Vector3 destination = transform.position;
        Vector3 legSource = transform.position;
        float distanceToTravel = GetComponent<Rigidbody>().velocity.magnitude * t;
        int nextMarkerIndex = nextMarker;
        float nextMarkerDist = (transform.position - levelMarkers[nextMarkerIndex]).magnitude;
        int numTurns = 0;

        while (distanceToTravel > nextMarkerDist && numTurns < maxTurns)
        {
            Vector3 leg = levelMarkers[nextMarkerIndex] - legSource;
            if (legSource == transform.position)
            {
                leg.y = 0;
            }
            destination += leg;
            distanceToTravel -= leg.magnitude;
            legSource = levelMarkers[nextMarkerIndex];
            if (nextMarkerIndex == levelMarkers.Count - 1)
            {
                return levelMarkers[nextMarkerIndex]; // Enemy will exit level by the end of the forecast
            }
            nextMarkerIndex++;
            nextMarkerDist = (transform.position - levelMarkers[nextMarkerIndex]).magnitude;
            numTurns++;
        }

        destination += (levelMarkers[nextMarkerIndex] - levelMarkers[nextMarkerIndex - 1]).normalized * distanceToTravel;

        return destination;
    }

    // updates scale and color
    void UpdateHealthBar()
    {
        float healthBarWidth = 7f;
        float healthBarMaxLength = 40f;
        float healthFraction = health / MAX_HEALTH;
        float scaleRate = 0.15f;
        if (health == MAX_HEALTH)
        {
            healthBar.transform.localScale = new Vector3(healthBarWidth, healthBarWidth, healthBarMaxLength);
            healthBar.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.green, healthFraction);
        }
        else
        {
            healthBar.transform.localScale = new Vector3(healthBarWidth, healthBarWidth,
                healthBar.transform.localScale.z - (healthBar.transform.localScale.z - healthFraction * healthBarMaxLength) * scaleRate);
            healthBar.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.green, healthBar.transform.localScale.z / healthBarMaxLength);

        }
    }

}
