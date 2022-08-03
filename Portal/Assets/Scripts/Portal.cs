using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour
{

    // Set in editor
    public Transform otherPortal;
    public Transform player;

    public Portal otherPortalScript;

    public bool ignoreNextCollision;

    void Start() {
        otherPortalScript = otherPortal.GetComponent<Portal>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Player") {
            if (ignoreNextCollision) {
                ignoreNextCollision = false;
            }
            else {
                otherPortalScript.ignoreNextCollision = true;
                player.position = otherPortal.position;
            }
        }
    }
}
