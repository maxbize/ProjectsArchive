using UnityEngine;
using System.Collections;

public class BulletTrace : MonoBehaviour {

    private float timeAlive = 0;
    private float timeToDeath = 1;

	// Update is called once per frame
	void Update () {
        timeAlive += Time.deltaTime;
        if (timeAlive > timeToDeath) {
            Destroy(gameObject);
        }
	}
}
