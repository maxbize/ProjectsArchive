using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Class that contains the bullet engine
public class BulletManager : MonoBehaviour {

    public Transform bulletPrefab;
    private int shotDistance = 1000;
    private GameManager gameManager;

    // TODO: Replace once you have different weapon types
    int baseDamage = 120;

	// Use this for initialization
	void Start () {
        gameManager = FindObjectOfType<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //[RPC]
    public void shoot(Vector3 origin, Vector3 dir, int playerId) {
        Transform bulletObj = (Transform)Instantiate(bulletPrefab, origin, Quaternion.FromToRotation(origin, origin + dir));
        bulletObj.GetComponent<Rigidbody>().velocity = dir * 3000;

        // Only the server will actually handle bullet damage
        if (true) {//Network.isServer) {
            int damage = baseDamage;
            List<RaycastHit> hits = Physics.RaycastAll(origin, dir, shotDistance).OrderBy(h => h.distance).ToList();
            foreach (RaycastHit hit in hits) {
                Debug.Log("Hit: " + hit.transform.name);
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Players")) {
                    Player hitPlayer = hit.transform.GetComponent<Player>();
                    if (gameManager.getPlayerFromId(playerId).team != hitPlayer.team) {
                        //hitPlayer.networkView.RPC("registerDamage", RPCMode.All, damage, playerId);
                    }
                    damage -= (int)(baseDamage * (1 - hit.transform.GetComponent<StoppingPower>().damageThroughMultiplier));
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Obstacles")) {
                    damage -= (int)(baseDamage * (1 - hit.transform.GetComponent<StoppingPower>().damageThroughMultiplier));
                }
            }
        }
    }
}
