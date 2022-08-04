using UnityEngine;
using System.Collections;

// This is the hook weapon. It grabs onto another player or scenery and pulls the player(s) together
public class Hook : MonoBehaviour {

	float speed = 1f;

	const int maxSegments = 6; // Including hook tip
	public Transform[] segments = new Transform[maxSegments];
	int numMovingSegments = 0;

	Vector2 spawnPos;
	float segmentSize = 0.25f;
	bool expanding = true;


	// Use this for initialization
	void Start () {
		spawnPos = segments[0].position;
		segments[0].GetComponent<Rigidbody2D>().velocity = new Vector2 (-1, 0) * speed;
		numMovingSegments++;
	}
	
	// Update is called once per frame
	void Update () {
		if (expanding) {
			if (((Vector2)segments[0].position - spawnPos).magnitude > segmentSize * numMovingSegments) {
				if (numMovingSegments < maxSegments) {
					segments[numMovingSegments].GetComponent<Rigidbody2D>().velocity = new Vector2(-1,0) * speed;
					numMovingSegments++;
				}
				else {
					for (int i = 0; i < maxSegments; i++) {
						segments[i].GetComponent<Rigidbody2D>().velocity = -segments[i].GetComponent<Rigidbody2D>().velocity;
					}
					expanding = false;
				}
			}
		}
		else {
			if (((Vector2)segments[0].position - spawnPos).magnitude < segmentSize * (numMovingSegments == 1 ? 0.1f : numMovingSegments - 1)) {
				if (numMovingSegments > 1) {
					numMovingSegments--;
					segments[numMovingSegments].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				}
				else {
					Destroy(gameObject);
				}
			}
		}
	}
}

/*
 * In order for the hook to look right, we'll need to draw and move the segments one by one.
 * Spawn a segment under the car, then move it forward until it has been displaced by its size.
 * Then spawn in a new segment and repeat, always pushing the old segments as well. Each segment
 * should maintain its velocity, meaning that if the car turns the segment should not. (Bonus) After 
 * the end of the hook hits an object, the hook should reel back in, becoming taught before any forces
 * are applied
 */