using UnityEngine;
using System.Collections;

public class ColoredCheckpoint : MonoBehaviour {

	public int order = 0;
    public LevelManager levelManager;
    Color defaultColor;

	// Use this for initialization
	void Start () {
        if (order == 0) {
            Debug.LogError("ERROR: Checkpoint created without an ID!");
        }
        levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        defaultColor = gameObject.transform.FindChild("check-1").GetComponent<SpriteRenderer>().color;
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Player") {
            Car car = other.transform.gameObject.GetComponent<Car>();
            if (levelManager.recordCheckpoint(order, car)) {
			    string lightName = "check-" + (car.playerNb + 1).ToString();
			    Transform light = gameObject.transform.FindChild(lightName);
                if (light) {
                    SpriteRenderer renderer = light.gameObject.GetComponent<SpriteRenderer>();
                    renderer.color = GameManager.PLAYER_COLORS[car.playerNb];
                }
            }
		}
	}

    public void resetLight(int player) {
        string lightName = "check-" + (player + 1).ToString();
        Transform light = gameObject.transform.FindChild(lightName);
        if (light)
        {
            SpriteRenderer renderer = light.gameObject.GetComponent<SpriteRenderer>();
            renderer.color = defaultColor;
        }
    }
}
