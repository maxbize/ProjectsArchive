using UnityEngine;
using System.Collections.Generic;

public class BallManager : MonoBehaviour {
     
    // Balls in the game
    List<Ball> balls;

	// Use this for initialization
	void Start () 
    {
        balls = new List<Ball>();
        SpawnBall();
	}
	 
	// Update is called once per frame
    void Update()
    {

	}

    void SpawnBall()
    {
        GameObject ballObj = (GameObject)GameObject.Instantiate(Resources.Load("Ball"));
        Ball ball = ballObj.AddComponent<Ball>();
        Light light = ballObj.AddComponent<Light>();
        light.type = LightType.Point;
        balls.Add(ball);
    }
}
