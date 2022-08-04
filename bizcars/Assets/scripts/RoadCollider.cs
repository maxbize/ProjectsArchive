using UnityEngine;
using System.Collections;

public class RoadCollider : MonoBehaviour {
	
	public float controlMod_road = 1F;
	public float controlMod_grass = 0.6F;

	Car parentCar;

	// Use this for initialization
	void Start () {
		parentCar = gameObject.transform.parent.gameObject.GetComponent<Car> ();
		parentCar.UpdateRoadControl (controlMod_road);
	}

	void OnTriggerEnter2D(Collider2D other) {
		OnTrigger(other.tag);
	}

	private void OnTriggerStay2D(Collider2D collision) {
		OnTrigger(collision.tag);
	}

	private void OnTrigger(string type) {
		switch (type) {
			case "Road":
				parentCar.UpdateRoadControl(controlMod_road);
				break;
			case "Grass":
				parentCar.UpdateRoadControl(controlMod_grass);
				break;
				/*default:
					Debug.LogError("Error: Unrecognized collider tag in RoadCollider::OnTriggerEnter: " + other.tag);
					break;*/
		}
	}
}
