using UnityEngine;
using System.Collections;

public class PortalGun : MonoBehaviour {

    private enum portalType { red, blue };

    private Transform bluePortal;
    private Transform redPortal;

    void Start() {
        bluePortal = GameObject.Find("Blue Portal").transform;
        redPortal = GameObject.Find("Red Portal").transform;
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            shootPortal(portalType.red);
        }

        else if (Input.GetMouseButtonDown(1)) {
            shootPortal(portalType.blue);
        }
	}

    private void shootPortal(portalType type) {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit)) {
            movePortal(type, hit.point);
        }
    }

    private void movePortal(portalType type, Vector3 position) {
        if (type == portalType.red) {
            redPortal.position = position;
        }
        else if (type == portalType.blue) {
            bluePortal.position = position;
        }
    }
}
